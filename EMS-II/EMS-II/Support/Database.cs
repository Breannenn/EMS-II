using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support
{
    /// <summary>
    /// This class provides support methods for accessing database files. 
    /// </summary>
    /// <remarks>
    /// Modules can use this class to create databases, find, update and add new records.
    /// To use, create a Database object with the database name as the parameter. This 
    /// can only be used with an existing database. New databases can be created using the
    /// static method. Exceptions will be thrown for File IO errors, database corruption errors,  
    /// and basic database validation errors (record size, duplicate keys etc).
    /// </remarks>
    public class Database
    {
        private String filename;                        /// the database file name
        private int recordSize;                         /// the maximum size of a record
        private String primaryKey;                      /// the primary key
        public Dictionary<String, String> database;    /// Dictionary of the database key value and record pairs
        private bool needRefresh = true;    /// indicates whether the database should be reloaded before doing stuff


        /// <summary>
        /// Checks/Sets up basic database requirements.
        /// </summary>
        /// <remarks>
        /// Checks for the EMS Databases folder and Database list file. It will create
        /// these if they do not exist. Need to run this function when starting up the
        /// EMS application.
        /// </remarks>
        /// <returns>true: successful, false: unsuccessful</returns>
        static public bool StartDatabaseSystem()
        {
            bool retVal = true;

            // Create a Databases folder if one does not exist.
            if (!Directory.Exists(SupportConstants.DATABASE_FOLDER))
            {
                try
                {
                    Directory.CreateDirectory(SupportConstants.DATABASE_FOLDER);
                }
                catch (Exception ex)
                {
                    string details = "Error: " + ex.Message;
                    Logging.LogMsg("Database", "StartDatabaseSystem", details);
                    retVal = false;
                }
            }
            else
            {
                // Creates an empty DatabaseList file if one does not exist.
                if (!File.Exists(SupportConstants.DATABASE_LIST))
                {
                    try
                    {
                        File.Create(SupportConstants.DATABASE_LIST);
                    }
                    catch (Exception ex)
                    {
                        string details = "Error: " + ex.Message;
                        Logging.LogMsg("Database", "StartDatabaseSystem", details);
                        retVal = false;
                    }
                }
            }

            if (retVal == true)
            {
                Logging.LogMsg("Database", "StartDatabaseSystem", "Started Database System.");
            }
            return retVal;
        }


        /// <summary>
        /// Create a new database file.
        /// </summary>
        /// <remarks>
        /// This function will create a new Database file and add the file to the Databases list
        /// as well as the parameters for the new Database file (file name, record size, and primary key).
        /// Can throw an ArgumentException if the database already exists.
        /// </remarks>
        /// <param name="databaseName">the name for the database</param>
        /// <param name="recordSize">the maximum size of a record in bytes</param>
        /// <param name="keyName">the key name eg. healthCardNumber</param>
        /// <returns>true: successful, false: unsuccessful</returns>
        static public bool CreateNewDatabase(String databaseName, int recordSize, String keyName)
        {
            string filepath = SupportConstants.DATABASE_FOLDER + "\\" + databaseName + ".txt";

            // Check if the database already exists. If so, throw an exception.
            if (File.Exists(filepath))
            {
                throw new ArgumentException("Database already exists.");
            }

            try
            {
                // Create the new database file and add to the database files list.
                File.Create(filepath);
                string line = databaseName + SupportConstants.FIELD_DELIM + recordSize.ToString()
                    + SupportConstants.FIELD_DELIM + keyName;
                FileSupport.WriteLine(SupportConstants.DATABASE_LIST, line);
                string details = "Created database " + databaseName;
                Logging.LogMsg("Database", "CreateNewDatabase", details);
            }
            catch (Exception e)
            {
                // There was an error. Delete the database if it exists at this point.
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                    string details = "Deleted database " + databaseName + "because of: " + e.Message;
                    Logging.LogMsg("Database", "CreateNewDatabase", details);
                }
                throw;
            }

            if (File.Exists(filepath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// The object can only be constructed for a database that exists in the Databases folder. Can throw
        /// FileNotFoundException, DirectoryNotFoundException, and Exception.
        /// </remarks>
        /// <param name="dbName">the name of the database to connect to</param>
        public Database(String dbName)
        {
            // filename cannot be blank, look for the file, if not found throw a
            // file not found exception
            try
            {
                // First look for the database directory. Then look for the file in the 
                // Databases folder. If the file exists, look for the database settings
                // in the appropriate file and set the filename and other details for the 
                // new object.
                if (Directory.Exists(SupportConstants.DATABASE_FOLDER))
                {
                    // next look for the file in the database directory
                    String fullpath = SupportConstants.DATABASE_FOLDER + "\\" + dbName + ".txt";
                    if (File.Exists(fullpath))
                    {
                        string[] info = GetDatabaseInfo(dbName);
                        if (info != null)
                        {
                            filename = info[0];
                            recordSize = Convert.ToInt32(info[1]);
                            primaryKey = info[2];
                        }
                        else
                        {
                            throw new Exception("Database file list corrupted. Cannot find database information.");
                        }
                        // Load the database into the database variable (Dictionary)
                        GetDatabase();
                        needRefresh = false;
                    }
                    else
                    {
                        FileNotFoundException fnfex = new FileNotFoundException("The specified database does not exist.");
                        throw fnfex;
                    }
                }
                else
                {
                    DirectoryNotFoundException dnfex = new DirectoryNotFoundException("An error occurred when accessing the Databases folder.");
                    throw dnfex;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Returns the name of the database attached to the object.
        /// </summary>
        /// <remarks>
        /// Returns the name of the database for the object as a string.
        /// </remarks>
        /// <returns>String: the name of the database for the object</returns>
        public String GetDatabaseName()
        {
            return filename;
        }


        /// <summary>
        /// Returns the size of a record in the database.
        /// </summary>
        /// <remarks>
        /// This method will return the size of a record in bytes for the database attached 
        /// to the object.
        /// </remarks>
        /// <returns>int: the size of the record in bytes</returns>
        public int GetRecordSize()
        {
            return recordSize;
        }


        /// <summary>
        /// Returns the primary key for the database.
        /// </summary>
        /// <remarks>
        /// This method will return the primary key for the record in the database attached 
        /// to the object.
        /// </remarks>
        /// <returns>String: the primary key for the database </returns>
        public String GetKey()
        {
            return primaryKey;
        }


        /// <summary>
        /// Search and return a record.
        /// </summary>
        /// <remarks>
        /// This method will search for a record in the attached database using the supplied search term.
        /// It will return the first record it finds in the database matching the search term. This method 
        /// uses ParseRecord() to parse the records for their primary key values for comparison. Other 
        /// modules use this function to search for a particular record.
        /// </remarks>
        /// <param name="searchTerm">String: the primary key value used to search for the record.</param>
        /// <returns>The record as a string. The method will return an empty string if no record was found.</returns>
        public String GetRecord(String searchTerm)
        {
            string record = "";
            if (needRefresh)
            {
                GetDatabase();
                needRefresh = false;
            }
            if (database.ContainsKey(searchTerm))
            {
                record = database[searchTerm];
            }
            return record;
        }


        /// <summary>
        /// Parse a record for the primary key value.
        /// </summary>
        /// <remarks>
        /// This method will parse a string looking for the first field in the record which holds its
        /// primary key value. The field delimiters are found in the SupportConstants class.
        /// </remarks>
        /// <param name="record"></param>
        /// <returns>String: the first field value</returns>
        protected String ParseRecord(String record)
        {
            // ADD CHECK FOR AN EMPTY STRING
            string[] splitRecord = record.Split(SupportConstants.FIELD_DELIM);
            return splitRecord[0];
        }


        /// <summary>
        /// Add a new record to the database.
        /// </summary>
        /// <remarks>
        /// Other modules use this method to add a new record into the database. This method will parse the
        /// record for its primary key value using ParseRecord(). It will also check that no other record 
        /// has the same value using GetRecord() before adding the new record to the end of the database file. 
        /// </remarks>
        /// <param name="newRecord"></param>
        /// <returns>bool: true for success, false for failure</returns>
        public bool AddRecord(String newRecord)
        {
            bool retVal = false;

            // Check if the newRecord has a key value that is unique to the database

            if (needRefresh)
            {
                // refresh the database list
                GetDatabase();
                needRefresh = false;
            }

            // Get the primary key value from newRecord and check if it is already in the database
            string[] splitRecord = newRecord.Split(SupportConstants.FIELD_DELIM);
            if (database.ContainsKey(splitRecord[0]))
            {
                string details = "User supplied record with a primary key value that already exists in the database " + filename;
                Logging.LogMsg("Database", "AddRecord", details);
                throw new Exception("Error: the primary key value already exists in the database.");
            }

            string filepath = SupportConstants.DATABASE_FOLDER + "\\" + filename + ".txt";
            try
            {
                FileSupport.WriteLine(filepath, newRecord);
                needRefresh = true;
                string details = "Added a new record to " + filename;
                Logging.LogMsg("Database", "AddRecord", details);
                retVal = true;
            }
            catch (Exception e)
            {
                string details = "Failed to add record. Error: " + e.Message;
                Logging.LogMsg("Database", "AddRecord", details);
            }
            return retVal;
        }


        /// <summary>
        /// Updates a specified record.
        /// </summary>
        /// <remarks>
        /// This method updates a specified record, indicated by the searchTerm, with new information. 
        /// It will check the primary key value in the newRecord and the search term to make sure there
        /// are no duplicates in the database (using ParseRecord() and GetRecord() ). If all is well, 
        /// the old record is replaced with the new one. It can be used by other modules.
        /// </remarks>
        /// <param name="searchTerm"></param>
        /// <param name="newRecord"></param>
        /// <returns></returns>
        public bool UpdateRecord(String searchTerm, String newRecord)
        {
            bool retVal = false;
            // ADD A CHECK FOR IF THE SEARCH TERM IS IN AN EXISTING RECORD
            if (needRefresh)
            {
                GetDatabase();
                needRefresh = false;
            }

            // Check if the new records' primary key value matches the searchTerm. If it does, simply 
            // replace the record in the database. If it doesn't, see if the new primary key is already
            // in the database. If it is, throw an exception because it is a duplicate key.
            string newKey = ParseRecord(newRecord);
            if (searchTerm != newKey)
            {
                if (database.ContainsKey(newKey))
                {
                    string details = "update aborted - duplicate primary key value in new record";
                    Logging.LogMsg("Database", "UpdateRecord", details);
                    throw new Exception("Error: This action would create duplicate primary keys.");
                }
                else
                {
                    AddRecord(newRecord);
                    retVal = true;
                    string details = "Added new record to " + filename;
                    Logging.LogMsg("Database", "UpdateRecord", details);
                }
            }
            else
            {
                string oldRecord = database[searchTerm];

                if (UpdateDatabaseFile(oldRecord, newRecord))
                {
                    database[searchTerm] = newRecord;
                    string details = "Updated record in " + filename;
                    Logging.LogMsg("Database", "UpdateRecord", details);
                    retVal = true;
                }
            }
            return retVal;
        }


        /// <summary>
        /// This method populates a Dictionary container with the databases' key values
        /// and records.
        /// </summary>
        /// <remarks>
        /// The key for the dictionary is the parsed key from a record. The dictionary value
        /// is the whole record as a string, including the key value. The dictionary is used
        /// for operations on the database.
        /// </remarks>
        /// <returns>true - successful, false - unsuccessful</returns>
        public bool GetDatabase()
        {
            bool retVal = false;
            database = new Dictionary<string, string>();
            string filepath = SupportConstants.DATABASE_FOLDER + "\\" + filename + ".txt";
            string[] raw = FileSupport.ReadAllLines(filepath);
            string[] splitRecord = null;
            string key = "";

            try
            {
                foreach (string s in raw)
                {
                    splitRecord = s.Split(SupportConstants.FIELD_DELIM);
                    key = splitRecord[0];
                    database.Add(key, s);
                }
                retVal = true;
                needRefresh = false;
            }
            catch (Exception e)
            {
                string details = "Failed to load database " + filename + ". With error: " + e.Message;
                Logging.LogMsg("Database", "GetDatabase", details);
                throw new Exception("An error occurred, failed to load database.");
            }

            return retVal;
        }

        /// <summary>
        /// This method will look in the DatabaseList.txt file for the database
        /// parameters.
        /// </summary>
        /// <param name="dbName">string - the database name</param>
        /// <returns>string array - the info in an array or an empty string if the
        /// database does not exist. </returns>
        protected string[] GetDatabaseInfo(string dbName)
        {
            string filepath = SupportConstants.DATABASE_LIST;
            string[] all = FileSupport.ReadAllLines(filepath);
            string[] line = null;
            bool found = false;
            int i = 0;

            while (!found && i < all.Length)
            {
                line = all[i].Split(SupportConstants.FIELD_DELIM);
                if (line[0] == dbName)
                {
                    found = true;
                }
                else
                {
                    i = i + 1;
                }
            }
            if (found == false)
            {
                line = null;
            }
            return line;
        }


        protected bool UpdateDatabaseFile(string oldRecord, string newRecord)
        {
            string filepath = SupportConstants.DATABASE_FOLDER + "\\" + filename + ".txt";
            string[] dbBuffer = FileSupport.ReadAllLines(filepath);
            bool retVal = false;

            try
            {
                for (int i = 0; i < dbBuffer.Length; i++)
                {
                    if (dbBuffer[i] == oldRecord)
                    {
                        dbBuffer[i] = newRecord;
                        break;
                    }
                }

                string tempPath = SupportConstants.DATABASE_FOLDER + "\\tempdb.txt";
                //File.Delete(tempPath);
                File.WriteAllLines(tempPath, dbBuffer);
                File.Replace(tempPath, filepath, null);

                string details = "Replaced database file (" + filepath + ")";
                Logging.LogMsg("Database", "UpdateDatabaseFile", details);

                retVal = true;
            }
            catch (Exception e)
            {
                string details = "Error: " + e.Message;
                Logging.LogMsg("Database", "UpdateDatabaseFile", details);
            }
            return retVal;
        }

        /// <summary>
        /// Search and return a record based on searching a particular field number.
        /// </summary>
        /// <remarks>
        /// This method will search for a record in the attached database using the supplied search term.
        /// It will return a list of records it finds in the database matching the search term. It will throw an
        /// ArgumentOutOfRange exception if you use a number outside the range of fields in the record.
        /// </remarks>
        /// <param name="searchTerm">String: the value used to search for the record.</param>
        /// <param name="fieldNumber">Int: the field number to compare the search term with (starts at index 0)</param>
        /// <returns>The record as a list of strings. The method will return an empty list if no record was found.</returns>
        public List<String> GetRecordList(String searchTerm, int fieldNumber)
        {
            List<String> matches = new List<String>();

            if (needRefresh)
            {
                GetDatabase();
                needRefresh = false;
            }
            foreach (KeyValuePair<string,string> pair in database)
            {
                string[] splitRecord = pair.Value.Split(SupportConstants.FIELD_DELIM);
                if(fieldNumber >= splitRecord.Length || fieldNumber < 0)
                {
                    throw new ArgumentOutOfRangeException("The field number is not a valid number for this database.");
                }
                if (searchTerm.Equals(splitRecord[fieldNumber], StringComparison.OrdinalIgnoreCase ))
                {
                    matches.Add(pair.Value);
                }
            }

            return matches;
        }


        /// <summary>
        /// Search and return a record based on searching a particular field number.
        /// </summary>
        /// <remarks>
        /// This method will search for a record in the attached database using the supplied search term.
        /// It will return the first record it finds in the database matching the search term. It will throw an
        /// ArgumentOutOfRange exception if you use a number outside the range of fields in the record.
        /// </remarks>
        /// <param name="searchTerm">String: the value used to search for the record.</param>
        /// <param name="fieldNumber">Int: the field number to compare the search term with (starts at index 0)</param>
        /// <returns>The first record it finds. It will return an empty string if no record was found.</returns>
        public String GetRecord(String searchTerm, int fieldNumber)
        {
            string match = "";

            if (needRefresh)
            {
                GetDatabase();
                needRefresh = false;
            }
            foreach (KeyValuePair<string, string> pair in database)
            {
                string[] splitRecord = pair.Value.Split(SupportConstants.FIELD_DELIM);
                if (fieldNumber >= splitRecord.Length || fieldNumber < 0)
                {
                    throw new ArgumentOutOfRangeException("The field number is not a valid number for this database.");
                }
                if (searchTerm.Equals(splitRecord[fieldNumber], StringComparison.OrdinalIgnoreCase))
                {
                    match = pair.Value;
                    break;
                }
            }

            return match;
        }
    }


}
