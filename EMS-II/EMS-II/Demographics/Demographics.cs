//using System;
//using System.IO;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;
//using Support;

///// \namespace Demographics
///// 
///// The Demographics namespace holds the PatientInfo class which has all validation and information on a patient required for the EMS program.
//namespace Demographics
//{
//    public class Globals
//    {
//        public const int phoneTriplet = 3;
//        public const int phoneQuad = 4;
//        public const int phoneLength = 10;
//        public const int HCNLength = 12;
//        public const int maxNameLen = 35;
//        public const int maxAddressLen = 95;
//        public const int maxCityLen = 40;
//        public const int dateLength = 8;
//        public const int maxDay = 31;
//        public const int maxMonth = 12;
//        public const int minYear = 1900;
//        public const int dayFormat = 2;
//        public const int monthFormat = 2;
//        public const int empty = 0;
//    }

//    /// \class HealthCard
//    /// 
//    /// The HealthCard class holds a healthcard number as well as the validation required for a healthcard number.
//    public class HealthCard
//    {
//        String healthCardNum;

//        /// \breif
//        /// This Ctor allows a new healthcard number to be instantiated
//        ///
//        public HealthCard(String newNumber)
//        {
//            if (validateNumber(newNumber))
//            {
//                healthCardNum = newNumber;
//            }
//        }

//        public override string ToString()
//        {
//            string retString = "";

//            if (healthCardNum != null)
//            {
//                string[] HCNValue = healthCardNum.Split(':');

//                retString = HCNValue[0];
//            }

//            return retString;
//        }

//        /// \method getHealthCardNum
//        /// 
//        ///This method returns the value of number so that anything outside the class can access the value of the variable
//        private String getHealthCardNum()
//        {
//            return healthCardNum;
//        }

//        /// \method setHealthCardNum
//        /// \param <b>newNumber</b> - String. Holds the new number that will be set as the number
//        /// 
//        /// \return Returns true if the number was set false otherwise
//        /// 
//        ///Sets the value of number to whatever the user enters providing it is a valid value
//        private bool setHealthCardNum(String newNumber)
//        {
//            bool retCode = false;

//            if (validateNumber(newNumber))
//            {
//                healthCardNum = newNumber;
//                retCode = true;
//            }

//            return retCode;
//        }

//        /// \method validateNumber
//        /// 
//        /// \param void
//        /// 
//        /// \return void
//        /// 
//        ///Confirms that HCN is a valid value to be entered into the Demographics database
//        public bool validateNumber(String newHCN)
//        {
//            bool retCode = false;

//            if (newHCN.Length == Globals.HCNLength)
//            {
//                String digits = newHCN.Substring(0, 10);
//                String alphas = newHCN.Substring(10, 2);

//                if (Utilities.IsNumeric(digits) && !Utilities.IsNumeric(alphas))
//                {
//                    retCode = true;
//                }
//                else
//                {
//                    //There was an issue with your health card format. Please enter 10 digits followed by 2 alpha characters
//                    retCode = false;
//                    throw new Exception("Health card number format is wrong");
//                }
//            }
//            else if (newHCN.Length < Globals.HCNLength)
//            {
//                //Too low 
//                retCode = false;
//                throw new Exception("Health card number is too short");

//            }
//            else if (newHCN.Length > Globals.HCNLength)
//            {
//                //Too high
//                retCode = false;
//                throw new Exception("Health card number is too long");
//            }
//            else
//            {
//                //Empty
//                retCode = false;
//                throw new Exception("Health card number is empty");
//            }

//            return retCode;
//        }

//    }

//    /// \class PatientInfo
//    /// 
//    /// The PatientInfo class hold all of the required data about a patient. It also provides all of the required validation for each of the datamembers.
//    public class PatientInfo
//    {
//        //Sets numbers for each of the parameters as indexes
//        public enum MemberAsIndex { hcn = 0, lastName, firstName, mInitial, dateBirth, sex, headOfHouse, addressLine1, addressLine2, city, province, numPhone };

//        #region DataMembers

//        HealthCard hcn = null;
//        string lastName = "";
//        string firstName = "";
//        char mInitial = '\0';
//        String dateBirth;
//        char sex = '\0';
//        HealthCard headOfHouse = null;
//        string addressLine1 = "";
//        string addressLine2 = "";
//        string city = "";
//        string province = "";
//        string numPhone = "";

//        public HealthCard HCN ///Holds the health card number
//		{
//            get
//            {
//                return hcn;
//            }
//            set
//            {
//                if (value.validateNumber(value.ToString()))
//                {
//                    //if (validateHOHPresent()) { } //Figure this out with Andy's module
//                    hcn = value;
//                }
//            }
//        }

//        public String LastName ///Holds the last name
//		{
//            get
//            {
//                return lastName;
//            }
//            set
//            {
//                if (validateLastName(value))
//                {
//                    lastName = value;
//                }
//            }
//        }

//        public String FirstName ///Holds the first name
//		{
//            get
//            {
//                return firstName;
//            }
//            set
//            {
//                if (validateFirstName(value))
//                {
//                    firstName = value;
//                }
//            }
//        }
//        public Char MInitial ///Holds the middle initial [NOT MANDATORY]
//		{
//            get
//            {
//                return mInitial;
//            }
//            set
//            {
//                if (validateMInitial(value))
//                {
//                    mInitial = value;
//                }
//            }
//        }
//        public String DateBirth ///Holds the date of birth
//		{
//            get
//            {
//                return dateBirth;
//            }
//            set
//            {
//                if (validateDateBirth(value))
//                {
//                    dateBirth = value;
//                }
//            }
//        }

//        public char Sex ///Holds the sex
//		{
//            get
//            {
//                return sex;
//            }
//            set
//            {
//                if (validateSex(value))
//                {
//                    sex = value;
//                }
//            }
//        }

//        public HealthCard HeadOfHouse ///Holds the head of house's Health Card Number
//		{
//            get
//            {
//                return headOfHouse;
//            }
//            set
//            {
//                if (validateHeadOfHouse(value))
//                {
//                    headOfHouse = value;
//                }
//            }
//        }

//        public String AddressLine1 ///Holds the address line 1
//		{
//            get
//            {
//                return addressLine1;
//            }
//            set
//            {
//                if (validateAddressLine1(value))
//                {
//                    addressLine1 = value;
//                }
//            }
//        }

//        public String AddressLine2 ///Holds the address line 2
//		{
//            get
//            {
//                return addressLine2;
//            }
//            set
//            {
//                if (validateAddressLine2(value))
//                {
//                    addressLine2 = value;
//                }
//            }
//        }

//        public String City ///Holds the city
//		{
//            get
//            {
//                return city;
//            }
//            set
//            {
//                if (validateCity(value))
//                {
//                    city = value;
//                }
//            }
//        }

//        public String Province ///Holds the province short form
//		{
//            get
//            {
//                return province;
//            }
//            set
//            {
//                if (validateProvince(value))
//                {
//                    province = value;
//                }
//            }
//        }

//        public String NumPhone ///Holds the phone number. Default value is ON
//		{
//            get
//            {
//                return numPhone;
//            }
//            set
//            {
//                if (validateNumPhone(value))
//                {
//                    numPhone = value;
//                }
//            }
//        }
//        #endregion

//        #region Ctors
//        /// \breif
//        /// This is the default Ctor that instantiates the object as null
//        ///
//        public PatientInfo()
//        {
//            hcn = null;
//            lastName = null;
//            firstName = null;
//            dateBirth = null;
//            sex = '\0';
//            addressLine1 = null;
//            addressLine2 = null;
//            city = null;
//            province = null;
//            numPhone = null;
//            mInitial = '\0';
//        }

//        /// \breif
//        /// This Ctor handles the non headOfHouse field option
//        ///
//        public PatientInfo(HealthCard newHCN, String newLastName, String newFirstName, String newDateBirth, char newSex, String newAddressLine1, String newAddressLine2, String newCity, String newProvince, String newNumPhone, char newMInitial)
//        {
//            if (newHCN.validateNumber(newHCN.ToString()))
//            {
//                hcn = newHCN;
//            }

//            if (validateLastName(newLastName))
//            {
//                lastName = newLastName;
//            }

//            if (validateFirstName(newFirstName))
//            {
//                firstName = newFirstName;
//            }

//            if (validateDateBirth(newDateBirth))
//            {
//                dateBirth = newDateBirth;
//            }

//            if (validateSex(newSex))
//            {
//                sex = newSex;
//            }

//            if (validateAddressLine1(newAddressLine1))
//            {
//                addressLine1 = newAddressLine1;
//            }

//            if (newAddressLine2 != "")
//            {
//                if (validateAddressLine2(newAddressLine2))
//                {
//                    addressLine2 = newAddressLine2;
//                }
//            }

//            if (validateCity(newCity))
//            {
//                city = newCity;
//            }

//            if (validateProvince(newProvince))
//            {
//                province = newProvince;
//            }
//            else
//            {
//                province = "ON";
//            }

//            if (validateNumPhone(newNumPhone))
//            {
//                numPhone = newNumPhone;
//            }

//            if (newMInitial != '\0')
//            {
//                if (validateMInitial(newMInitial))
//                {
//                    mInitial = newMInitial;
//                }
//            }
//        }

//        /// \breif
//        /// This Ctor handles the headOfHouse field option
//        ///
//        public PatientInfo(HealthCard newHCN, String newLastName, String newFirstName, String newDateBirth, char newSex, HealthCard newHeadOfHouse, char newMInitial)
//        {
//            if (newHCN.validateNumber(newHCN.ToString()))
//            {
//                hcn = newHCN;
//            }

//            if (validateLastName(newLastName))
//            {
//                lastName = newLastName;
//            }

//            if (validateFirstName(newFirstName))
//            {
//                firstName = newFirstName;
//            }

//            if (validateDateBirth(newDateBirth))
//            {
//                dateBirth = newDateBirth;
//            }

//            if (validateSex(newSex))
//            {
//                sex = newSex;
//            }

//            validateHeadOfHouse(newHeadOfHouse);

//            if (newMInitial != '\0')
//            {
//                if (validateMInitial(newMInitial))
//                {
//                    mInitial = newMInitial;
//                }
//            }
//        }
//        #endregion

//        #region Validation

//        /// \method validateLastName
//        /// 
//        ///Confirms that lastName is a valid value to be entered into the Demographics database 
//        public bool validateLastName(String newLName)
//        {
//            bool retCode = false;

//            if (newLName != "")
//            {
//                if (newLName.Length <= Globals.maxNameLen)
//                {
//                    if (Regex.IsMatch(newLName, @"^[a-zA-Z '-]+$"))
//                    {
//                        retCode = true;
//                    }
//                    else
//                    {
//                        retCode = false;
//                        //Enter a name containing only alpha characters with no digits or special characters
//                        throw new Exception("Last name can only contain alpha characters");
//                    }
//                }
//                else
//                {
//                    throw new Exception("Last name that is too long");
//                }
//            }
//            else
//            {
//                throw new Exception("Last name field is empty");
//            }

//            return retCode;
//        }

//        /// \method validateFirstName
//        /// 
//        ///Confirms that firstName is a valid value to be entered into the Demographics database 
//        public bool validateFirstName(String newFName)
//        {
//            bool retCode = false;

//            Regex lastNameCheck = new Regex(@"^[a-zA-Z '-]+$");
//            Match lastNameMatch = lastNameCheck.Match(newFName);

//            if (newFName != "")
//            {
//                if (newFName.Length < Globals.maxNameLen)
//                {
//                    if (lastNameMatch.Success)
//                    {
//                        retCode = true;
//                    }
//                    else
//                    {
//                        retCode = false;
//                        //Enter a name containing only alpha characters with no digits or special characters
//                        throw new Exception("First name can only contain alpha characters");
//                    }
//                }
//                else
//                {
//                    throw new Exception("First name that is too short");
//                }
//            }
//            else
//            {
//                throw new Exception("First name field is empty");
//            }

//            return retCode;
//        }

//        /// \method validateMInitial
//        /// 
//        ///Confirms that mInitial is a valid value to be entered into the Demographics database 
//        public bool validateMInitial(char newMInitial)
//        {
//            bool retCode = false;

//            if (!Utilities.IsNumeric(newMInitial))
//            {
//                retCode = true;
//            }
//            else
//            {
//                throw new Exception("Middle initial can only contain an alpha character");
//            }

//            return retCode;
//        }

//        /// \method validateDateBirth
//        /// 
//        ///Confirms that the Date of Birth is a valid value to be entered into the Demographics database 
//        public bool validateDateBirth(String birthDate)
//        {
//            bool retCode = false;

//            //Make sure the string is not empty
//            if (birthDate != "")
//            {
//                //Remove any / from the string
//                if (birthDate.Contains("/"))
//                {
//                    string[] birthParts = birthDate.Split('/');

//                    birthDate = "";
//                    foreach (string parts in birthParts)
//                    {
//                        birthDate += parts;
//                    }
//                }

//                //Remove any - from the string
//                if (birthDate.Contains("-"))
//                {
//                    string[] birthParts = birthDate.Split('-');

//                    birthDate = "";
//                    foreach (string parts in birthParts)
//                    {
//                        birthDate += parts;
//                    }
//                }

//                //Make sure the length of the string is correct
//                if (birthDate.Length == Globals.dateLength)
//                {
//                    //Get only the Day from the string
//                    string day = birthDate.Substring(0, 2);
//                    if (day.Length < Globals.dayFormat)
//                    {
//                        day = day.Insert(0, "0");
//                    }

//                    //Get only the Month from the string
//                    string month = birthDate.Substring(2, 2);
//                    if (month.Length < Globals.monthFormat)
//                    {
//                        month = month.Insert(0, "0");
//                    }

//                    //Get only the Year from the string
//                    string year = birthDate.Substring(4, 4);

//                    //Check if the string is only numeric
//                    if (Utilities.IsNumeric(birthDate))
//                    {
//                        //Check if the day is 
//                        int dayVal = int.Parse(day);
//                        if ((dayVal <= Globals.maxDay) && (dayVal > Globals.empty))
//                        {
//                            int monthVal = int.Parse(month);
//                            if ((monthVal <= Globals.maxMonth) && (monthVal > Globals.empty))
//                            {
//                                int yearVal = int.Parse(year);
//                                int currentYear = DateTime.Now.Year;
//                                if ((yearVal <= currentYear) && (yearVal > Globals.minYear))
//                                {
//                                    try
//                                    {
//                                        DateTime DOB = new DateTime(yearVal, monthVal, dayVal);
//                                        retCode = true;
//                                    }
//                                    catch (Exception invalidBirthday)
//                                    {
//                                        throw new Exception(invalidBirthday.ToString());
//                                    }
//                                }
//                                else
//                                {
//                                    //Incorrect Year
//                                    throw new Exception("Year format is incorrect");
//                                }
//                            }
//                            else
//                            {
//                                //Incorrect Month
//                                throw new Exception("Month format is incorrect");
//                            }
//                        }
//                        else
//                        {
//                            //Incorrect Day
//                            throw new Exception("Day format is incorrect");
//                        }
//                    }
//                    else
//                    {
//                        //Only digits allowed
//                        throw new Exception("Please enter only numeric digits");
//                    }
//                }
//                else
//                {
//                    //Wrong Format
//                    throw new Exception("Birthday is in the incorrect format");
//                }
//            }
//            else
//            {
//                //Birthdate empty
//                throw new Exception("Birthdat field is empty");
//            }

//            return retCode;
//        }

//        /// \method validateSex
//        /// 
//        ///Confirms that sex is a valid value to be entered into the Demographics database 
//        public bool validateSex(char newSex)
//        {
//            bool retCode = false;

//            newSex = char.ToUpper(newSex);
//            if ((newSex == 'M') || (newSex == 'F') || (newSex == 'H') || (newSex == 'I'))
//            {
//                retCode = true;
//            }
//            else
//            {
//                throw new Exception("Sex not valid");
//            }

//            return retCode;
//        }

//        /// \method validateHeadOfHouse
//        /// 
//        ///Confirms that headOfHouse is a valid value to be entered into the Demographics database 
//        public bool validateHeadOfHouse(HealthCard newHOH)
//        {
//            bool retCode = false;

//            string newHOHStr = newHOH.ToString();

//            // build search query
//            Pair HOHQuery = new Pair(FileIO.Fields[(int)FileIO.Table.Patients, (int)FileIO.PatientField.HCN], newHOHStr);
//            string headHouseQuery = HOHQuery.Values[0];

//            try
//            {
//                // Check if its in the database
//                Pair HOHRecord = new Pair(FileIO.SearchByKey(FileIO.Table.Patients, headHouseQuery));

//                // take HOH's values and put them in this object
//                headOfHouse = newHOH;
//                AddressLine1 = HOHRecord.Values[(int)(FileIO.PatientField.addressLine1)];
//                AddressLine2 = HOHRecord.Values[(int)(FileIO.PatientField.addressLine2)];
//                City = HOHRecord.Values[(int)(FileIO.PatientField.city)];
//                Province = HOHRecord.Values[(int)(FileIO.PatientField.province)];
//                NumPhone = HOHRecord.Values[(int)(FileIO.PatientField.numPhone)];
//            }
//            catch
//            {
//                retCode = false;
//            }

//            return retCode;
//        }

//        /// \method validateAddressLine1
//        /// 
//        ///Confirms that addressLine1 is a valid value to be entered into the Demographics database 
//        public bool validateAddressLine1(String newAddr1)
//        {
//            bool retCode = false;

//            Regex addressCheck = new Regex(@"^[1-9]+ [A-Za-z., -]+ [A-Za-z.,]+$"); //////////////////////////////////////////////////////REGEX DOESN'T WORK
//            Match addressMatch = addressCheck.Match(newAddr1);

//            if (newAddr1.Length <= Globals.maxAddressLen)
//            {
//                if (addressMatch.Success)
//                {
//                    retCode = true;
//                }
//                else
//                {
//                    retCode = false;
//                    //Enter a name containing only alpha characters with no digits or special characters
//                    throw new Exception("Invalid Address Line 1");
//                }
//            }
//            else
//            {
//                throw new Exception("Address Line 1 is too long");
//            }

//            return retCode;
//        }

//        /// \method validateAddressLine2
//        /// 
//        ///Confirms that addressLine2 is a valid value to be entered into the Demographics database 
//        public bool validateAddressLine2(String newAddr2)
//        {
//            bool retCode = false;

//            Regex addrCheck = new Regex(@"^[a-zA-Z0-9 ,'-.]+$"); /////////////////////////////////////////////////////////////////////DOUBLE CHECK THIS REGEX
//            Match addrMatch = addrCheck.Match(newAddr2);

//            if (newAddr2.Length < Globals.maxAddressLen)
//            {
//                if (addrMatch.Success)
//                {
//                    retCode = true;
//                }
//                else
//                {
//                    retCode = false;
//                    //Enter a name containing only alpha characters with no digits or special characters
//                    throw new Exception("Invalid Address Line 2");
//                }
//            }
//            else
//            {
//                throw new Exception("Address Line 2 is too long");
//            }

//            return retCode;
//        }

//        /// \method validateCity
//        /// 
//        ///Confirms that city is a valid value to be entered into the Demographics database 
//        public bool validateCity(String newCity)
//        {
//            bool retCode = false;

//            Regex cityCheck = new Regex(@"^[a-zA-Z '-]+$");
//            Match cityMatch = cityCheck.Match(newCity);

//            if (newCity.Length <= Globals.maxCityLen)
//            {
//                if (cityMatch.Success)
//                {
//                    retCode = true;
//                }
//                else
//                {
//                    retCode = false;
//                    //Enter a name containing only alpha characters with no digits or special characters
//                    throw new Exception("City name can only contain alpha characters");
//                }
//            }
//            else
//            {
//                throw new Exception("City name is invalid");
//            }

//            return retCode;
//        }

//        /// \method validateProvince
//        /// 
//        ///Confirms that province is a valid value to be entered into the Demographics database. Supports all 13 
//        ///provinces and territories in Canada. If short form is entered, program validates as normal but if full name
//        ///is entered, the input is converted to the short form.
//        public bool validateProvince(String newProvince)
//        {
//            bool retCode = false;

//            String[] provinces = File.ReadAllLines(FilePaths.provinceFile);
//            List<String> listTest = new List<String>(provinces);

//            newProvince = newProvince.ToUpper();
//            if (listTest.Contains(newProvince))
//            {
//                retCode = true;

//                if (newProvince == "ONTARIO")
//                {
//                    newProvince = "ON";
//                }
//                else if (newProvince == "QUEBEC")
//                {
//                    newProvince = "QC";
//                }
//                else if (newProvince == "BRITISH COLUMBIA")
//                {
//                    newProvince = "BC";
//                }
//                else if (newProvince == "ALBERTA")
//                {
//                    newProvince = "AB";
//                }
//                else if (newProvince == "MANITOBA")
//                {
//                    newProvince = "MB";
//                }
//                else if (newProvince == "SASKATCHEWAN")
//                {
//                    newProvince = "SK";
//                }
//                else if (newProvince == "NOVA SCOTIA")
//                {
//                    newProvince = "NS";
//                }
//                else if (newProvince == "NEW BRUNSWICK")
//                {
//                    newProvince = "NB";
//                }
//                else if (newProvince == "NEWFOUNDLAND AND LABRADOR")
//                {
//                    newProvince = "NL";
//                }
//                else if (newProvince == "PRINCE EDWARD ISLAND")
//                {
//                    newProvince = "PE";
//                }
//                else if (newProvince == "NORTHWEST TERRITORIES")
//                {
//                    newProvince = "NT";
//                }
//                else if (newProvince == "NUNAVUT")
//                {
//                    newProvince = "NU";
//                }
//                else if (newProvince == "YUKON")
//                {
//                    newProvince = "YT";
//                }
//            }
//            else
//            {
//                //This input is not allowed
//                throw new Exception("Invalid province");
//            }

//            return retCode;
//        }

//        /// \method validateNumPhone
//        /// 
//        ///Confirms that numPhone is a valid value to be entered into the Demographics database. Supports phone numbers
//        ///all together (8149589914), delimited by ' ' (814 958 9914) or delimited by '-' (814-958-9914). Canadian and American
//        ///area codes only. Area codes are stored in a file and cross checked with user input.
//        public bool validateNumPhone(String newPhone)
//        {
//            bool retCode = false;

//            String[] areaCodes = File.ReadAllLines(FilePaths.areaCodeFile);
//            List<String> listTest = new List<String>(areaCodes);

//            string[] reconstructPhone;
//            if (newPhone.Contains(" "))
//            {
//                reconstructPhone = newPhone.Split(' ');

//                if (reconstructPhone.Length == Globals.phoneTriplet)
//                {
//                    newPhone = reconstructPhone[0] + reconstructPhone[1] + reconstructPhone[2];
//                }
//            }

//            string[] numberSplit;
//            if (newPhone.Contains("-"))
//            {
//                numberSplit = newPhone.Split('-');

//                if (numberSplit.Length == Globals.phoneTriplet)
//                {
//                    if (listTest.Contains(numberSplit[0]))
//                    {
//                        String testLength = numberSplit[1].ToString();
//                        if (testLength.Length == Globals.phoneTriplet)
//                        {
//                            testLength = numberSplit[2].ToString();
//                            if (testLength.Length == Globals.phoneQuad)
//                            {
//                                newPhone = numberSplit[0] + numberSplit[1] + numberSplit[2];
//                                retCode = true;
//                            }
//                        }
//                    }
//                }
//            }
//            else
//            {
//                if (newPhone.Length == Globals.phoneLength)
//                {
//                    String areaCode = newPhone.Substring(0, 3);
//                    if (listTest.Contains(areaCode))
//                    {
//                        retCode = true;
//                    }
//                }
//                else
//                {
//                    if (newPhone.Length > Globals.phoneLength)
//                    {
//                        //Too many numbers
//                    }
//                    else if (newPhone.Length < Globals.phoneLength)
//                    {
//                        //Too few numbers
//                    }
//                }
//            }

//            if (retCode == false)
//            {

//                throw new Exception("Incorrect phone format");
//            }

//            return retCode;
//        }
//        #endregion

//        /// \method insert
//        /// 
//        /// \param PatientInfo insertPatient
//        /// 
//        /// \return void
//        /// 
//        ///This method takes a object of PatientInfo and inserts it into the patient database
//        public bool insert()
//        {
//            bool inserted = false;

//            return inserted;
//        }

//        /// \method update
//        /// 
//        /// \param PatientInfo insertPatient
//        /// 
//        /// \return void
//        /// 
//        ///This method takes a object of PatientInfo and updates it in the patient database
//        public bool update()
//        {
//            bool updated = false;

//            return updated;
//        }

//        /// \method search
//        /// 
//        /// \param PatientInfo insertPatient
//        /// 
//        /// \return void
//        /// 
//        ///This method takes a object of PatientInfo and searches for it in the patient database
//        public bool search(HealthCard searchPatient)
//        {
//            bool found = false;

//            return found;
//        }

//        /// \method ToString
//        /// 
//        /// \param void
//        /// 
//        /// \return void
//        /// 
//        ///This method Converts the object to a formatted string for the database to read
//        public override String ToString()
//        {
//            string hohPlaceholderValue = "";
//            string hohPlaceholderKey = "";

//            if (headOfHouse != null && !headOfHouse.ToString().Equals(""))
//            {
//                hohPlaceholderValue = headOfHouse.ToString() + " ";
//                hohPlaceholderKey = "headOfHouse ";
//            }

//            String PersonInfoString = "`" + hcn.ToString() + "` `" + lastName + "` `" + firstName + "` `" + mInitial +
//                                    "` `" + dateBirth + "` `" + sex + "` `" + hohPlaceholderValue + "` `" + addressLine1 +
//                                    "` `" + addressLine2 + "` `" + city + "` `" + province + "` `" + numPhone +
//                                    "` | `HCN` `lastName` `firstName` `mInitial` `dateBirth` `sex` `headOfHouse" +
//                                    "` `addressLine1` `addressLine2` `city` `province` `numPhone`";

//            return PersonInfoString;
//        }

//        /// \method retrieveHOHReport
//        /// 
//        /// \param PatientInfo insertPatient
//        /// 
//        /// \return void
//        /// 
//        ///This method Retrieves info on all HOH Relationships so it can be displayed to the user
//        public List<Pair> retrieveHOHReport(HealthCard headHouse)
//        {
//            List<Pair> allHouseMembers = new List<Pair>();

//            if (headHouse == null)
//            {
//                throw new NullReferenceException("This patient has no associated family members");
//            }
//            else
//            {

//                // build query to seach brothers and sisters of headOfHouse
//                Pair hohQuery = new Pair(FileIO.patientFields[(int)FileIO.PatientField.headOfHouse], headHouse.ToString());

//                // get query results from database
//                PairList hohQueryResults = new PairList(FileIO.SearchMany(FileIO.Table.Patients, hohQuery.RawData));
//                int hohNumResults = hohQueryResults.Count();

//                // iterate through all found records
//                for (int i = 0; i < hohNumResults; ++i)
//                {
//                    allHouseMembers.Add(hohQueryResults.pairs[i]);
//                }
//            }

//            return allHouseMembers;
//        }

//        /// \method checkForNullDataMembers
//        /// 
//        /// \param void
//        /// 
//        /// \return bool nullsFound. true if nulls were found false otherwise
//        /// 
//        ///This method checks if the object contained any nulls
//        public bool checkForNullDataMembers()
//        {
//            bool nullsFound = false;

//            if (hcn == null)
//            {
//                nullsFound = true;
//            }

//            if (lastName == null || firstName == null)
//            {
//                nullsFound = true;
//            }

//            if (dateBirth == null)
//            {
//                nullsFound = true;
//            }

//            if (sex == '\0')
//            {
//                nullsFound = true;
//            }

//            if (addressLine1 == null || addressLine2 == null)
//            {
//                nullsFound = true;
//            }

//            if (city == null)
//            {
//                nullsFound = true;
//            }

//            if (province == null)
//            {
//                nullsFound = true;
//            }

//            if (numPhone == null)
//            {
//                nullsFound = true;
//            }

//            return !nullsFound;
//        }

//        /// \method updateByIndex
//        /// 
//        /// \param MemberAsIndex index, String value
//        /// 
//        /// \return void
//        /// 
//        ///This method inserts a value given into the PersonInfo object based on the index provided
//        public void updateByIndex(MemberAsIndex index, String value)
//        {
//            switch (index)
//            {
//                case MemberAsIndex.hcn:
//                    HealthCard health = new HealthCard(value);
//                    this.HCN = health;
//                    break;
//                case MemberAsIndex.lastName:
//                    this.LastName = value;
//                    break;
//                case MemberAsIndex.firstName:
//                    this.FirstName = value;
//                    break;
//                case MemberAsIndex.mInitial:
//                    if (value.Length > 1)
//                    {
//                        throw new Exception("Middle initial too long");
//                    }
//                    this.mInitial = value[0];
//                    break;
//                case MemberAsIndex.dateBirth:
//                    this.DateBirth = value;
//                    break;
//                case MemberAsIndex.sex:
//                    if (value.Length > 1)
//                    {
//                        throw new Exception("Sex too long");
//                    }
//                    this.Sex = value[0];
//                    break;
//                case MemberAsIndex.headOfHouse:
//                    HealthCard hohHealth = new HealthCard(value);
//                    this.HeadOfHouse = hohHealth;
//                    break;
//                case MemberAsIndex.addressLine1:
//                    this.AddressLine1 = value;
//                    break;
//                case MemberAsIndex.addressLine2:
//                    this.AddressLine2 = value;
//                    break;
//                case MemberAsIndex.city:
//                    this.City = value;
//                    break;
//                case MemberAsIndex.province:
//                    this.Province = value;
//                    break;
//                case MemberAsIndex.numPhone:
//                    this.NumPhone = value;
//                    break;
//            }
//        }
//    }
//}