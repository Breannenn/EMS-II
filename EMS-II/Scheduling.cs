using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Demographics;
using Support;

namespace SchedulingLib
{
	/// <brief>Class to store appointments and related information.  Provides validation and an interface to the I/O module</brief>
	/// <summary>
	/// This class throws exceptions on bad arguements for buisiness logic.  
	/// Assumes input level validation is done at the UI layer.
	/// Provides an overridden ToString that can convert an object to a stream for writing to a file
	/// </summary>
	public class Appointment
	{
		//#region Data Members
		///// <summary>
		///// Public data members for the object
		///// </summary>
		//public HealthCard patientHCN { get; set; } /// Patient Health Card Number
		//public HealthCard careGiverHCN { get; set; } /// Care giver HCN
		//public DateTime dateTime { get; private set; } /// Date and time of the appointment.  Setter throws exception when time given is not a defined constant
		//public StatusValues Status { get; private set; } /// reflects if the object accurately represents an appointment in the database
		//public bool IsSynced { get; private set; } /// reflects if the object accurately represents an appointment in the database

		///// <summary>
		///// Public Constants, readonly, enums
		///// </summary>
		//public readonly int duration = 60; /// Length of appointment in MINUTES		
		//private const string keyFormat = "yyyy-MM-dd HH";
		//public enum StatusValues { Available = 0, Booked, Cancelled }; /// Status of appointment

		///// <summary>
		///// Private Constants, readonly, enums
		///// </summary>
		//private readonly int[] APPT_WEEKDAY_TIMES = new int[] { 9, 10, 11, 1, 2, 3 };
		//private readonly int[] APPT_WEEKEND_TIMES = new int[] { 10, 11 };
		//#endregion

		//#region Constructors
		//public Appointment(DateTime dateAndTime, HealthCard patient, StatusValues val)
		//{
		//	dateTime = dateAndTime;
		//	Status = val;
		//	patientHCN = patient;
		//	careGiverHCN = null;
		//	IsSynced = false;
		//}

		//public Appointment(DateTime dateAndTime, HealthCard patient, HealthCard careGiver, StatusValues val)
		//{
		//	dateTime = dateAndTime;
		//	Status = val;
		//	patientHCN = patient;
		//	careGiverHCN = careGiver;
		//	IsSynced = false;
		//}

		//public Appointment(DateTime dateAndTime)
		//{
		//	try
		//	{
		//		dateTime = dateAndTime;
		//		// get encoded record from database
		//		Pair result = new Pair(FileIO.SearchByKey(FileIO.Table.Appointments, dateTime.ToString(keyFormat)));
		//		string resultStatus = result.Values[(int)FileIO.AppointmentField.Status];

		//		patientHCN = new HealthCard(result.Values[(int)FileIO.AppointmentField.Patient]);
		//		if (!result.Values[(int)FileIO.AppointmentField.CareGiver].Equals(""))
		//		{
		//			careGiverHCN = new HealthCard(result.Values[(int)FileIO.AppointmentField.CareGiver]);
		//		}

		//		if (resultStatus.Equals("Booked"))
		//		{
		//			Status = StatusValues.Booked;
		//		}
		//		else if (resultStatus.Equals("Available"))
		//		{
		//			Status = StatusValues.Available;
		//		}
		//		else if (resultStatus.Equals("Cancelled"))
		//		{
		//			Status = StatusValues.Cancelled;
		//		}

		//		IsSynced = true;
		//	}
		//	catch (Exception e)
		//	{
		//		Logging.Log("Appointment", "CheckSlot", "Pair Construction and FileIO Call", e.Message);
		//		IsSynced = false;
		//		patientHCN = null;
		//		careGiverHCN = null;
		//		Status = StatusValues.Available;
		//	}
		//}

		//#endregion

		//#region General Methods
		///// <summary>
		///// Formats string for file I/O.  Takes all relevant data members and transforms them into key-value strings
		///// </summary>
		///// <returns>Formatted string</returns>
		//public string[][] ToKeyValue()
		//{
		//	/// Constants defined for code clarity
		//	const int NUM_VARS = 4;

		//	const int KEY_IDX = 0;
		//	const int VALUE_IDX = 1;

		//	const int PATIENT_IDX = 0;
		//	const int CAREG_IDX = 1;
		//	const int DATE_IDX = 2;
		//	const int STATUS_IDX = 3;

		//	string[][] EncodedString = new string[2][] {
		//		new string[NUM_VARS] { "", "", "", "" },
		//		new string[NUM_VARS] { "", "", "", "" }
		//		};

		//	try
		//	{
		//		EncodedString[KEY_IDX][PATIENT_IDX] = "patient";
		//		if (patientHCN == null)
		//		{
		//			EncodedString[VALUE_IDX][PATIENT_IDX] = "";
		//		}
		//		else
		//		{
		//			EncodedString[VALUE_IDX][PATIENT_IDX] = patientHCN.ToString();
		//		}

		//		EncodedString[KEY_IDX][CAREG_IDX] = "careGiver";
		//		if (careGiverHCN == null)
		//		{
		//			EncodedString[VALUE_IDX][CAREG_IDX] = "";
		//		}
		//		else
		//		{
		//			EncodedString[VALUE_IDX][CAREG_IDX] = careGiverHCN.ToString();
		//		}

		//		EncodedString[KEY_IDX][DATE_IDX] = "dateTime";
		//		EncodedString[VALUE_IDX][DATE_IDX] = dateTime.ToString(keyFormat);

		//		EncodedString[KEY_IDX][STATUS_IDX] = "status";
		//		EncodedString[VALUE_IDX][STATUS_IDX] = Status.ToString();
		//	}
		//	catch (Exception e)
		//	{
		//		Logging.Log("Appointment", "ToKeyValue", "Serialization of object", e.Message);
		//		throw e;
		//	}

		//	return EncodedString;
		//}
		//#endregion

		//#region DB Methods
		///// <summary>
		///// Accesses the schedule database and updates a slot with different information
		///// </summary>
		//public enum SyncResult { Success = 0, Update, Create, Error, IsSynced }
		//public FileIO.DBSyncStatus Sync()
		//{
		//	const int KEY_IDX = 0; /// Element 0 of serializedThis - represents key of key value pair
		//	const int VAL_IDX = 1; /// Element 1 of serializedThis - represents value of key value pair
		//	string[][] serializedThis = this.ToKeyValue();

		//	FileIO.DBSyncStatus result = FileIO.Sync(FileIO.Table.Appointments, serializedThis[KEY_IDX], serializedThis[VAL_IDX]);

		//	return result;
		//}
		//private const int APPT_COUNT_WEEKEND = 2;
		//private const int APPT_COUNT_WEEKDAY = 6;

		//public static List<Appointment> GetDay(DateTime date)
		//{
		//	date = Common.ToHour(date, 1);

		//	List<Appointment> appointments = new List<Appointment>();
		//	int numAppts = -1;
		//	int dayOfWeek = Common.GetDayIndex(date);

		//	if (dayOfWeek == (int)DayOfWeek.Saturday ||
		//		dayOfWeek == (int)DayOfWeek.Sunday)
		//	{
		//		numAppts = APPT_COUNT_WEEKEND;
		//	}
		//	else
		//	{
		//		numAppts = APPT_COUNT_WEEKDAY;
		//	}

		//	for (int i = 0; i < numAppts; i++)
		//	{
		//		appointments.Add(new Appointment(date));
		//		date = date.AddHours(1);

		//		//DateTime queryTime = date;
		//		//queryTime = queryTime.AddHours(i + 1);
		//	}

		//	return appointments;
		//}

		//public static List<Appointment> GetWeek(DateTime date)
		//{
		//	List<Appointment> appointments = new List<Appointment>();
		//	date = Common.BackToSunday(date);

		//	// get all appointment slots from all days in the week
		//	for (int i = 0; i < 7; i++)
		//	{
		//		appointments.AddRange(GetDay(date));
		//		date = date.AddDays(1);
		//	}

		//	return appointments;
		//}

		//public static List<Appointment> GetMonth(DateTime date)
		//{
		//	List<Appointment> appointments = new List<Appointment>();

		//	// prepare our for loop
		//	date = Common.BackToMonthStart(date);
		//	int numDays = Common.NumDaysInMonth(date);

		//	// get all appointment slots from all days in the month
		//	for (int i = 0; i < numDays; ++i)
		//	{
		//		appointments.AddRange(GetDay(date));
		//		date = date.AddDays(1);
		//	}


		//	return appointments;
		//}
		//#endregion
	}
}

// To Do
// Method for inserting a month of appointments
// Support for billing codes (Appointment must be able to hold billing codes)
// Health card validation service application (see specs)

// Questions for Russ:
// Are the lengths of apoointments fixed or variable? What are the length(s) that we are required to support