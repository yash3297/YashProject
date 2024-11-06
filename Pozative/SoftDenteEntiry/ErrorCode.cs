using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    public enum ErrorCode
    {
        Success,
        Failure,
        NotSupported,
        NotAllowed,
        NotLoggedIn,
        DBInitFailure,
        DBVersionError,
        DBInvalidPath,
        LoginFailed,
        InvalidID,
        InvalidPatientID,
        InvalidUserID,
        RecordLocked,
        AddRecordFailed,
        UpdateRecordFailed,
        InteropDataError,
        DBServiceError,
        DBLocked,
        BlockSlotExists,
        AppointmentExists,
        InvalidStartTime,
        InvalidSlotLength,
        NoAvailableSlot,
        NoTreatmentCode,
        DurationNotDivisible,
        Blackout,
        DBLicensing,
        DurationTooLong,
        NotEnoughContiguiousSpace,
        NoFileNameSpecified,
        CaptureDateIncorrectlyFormatted,
        NoCaptureDateHeaderFound,
        NoCaptureDateFound,
        NoUnitOfMeasurementHeaderFound,
        NoUnitOfMeasurementFound,
        NoDataHeadersFound,
        BadDataFound,
        NoDataFound,
        NoDataOrWorksheetFound,
        ErrorOpeningOfficeObject,
    }
}
