using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Enums
{
    public enum BackendStatusCodeEnum
    {
        StateInvalidUrl = 1,
        StateDbError,
        StateInvalidMethod,
        StateNotAuthorized,
        StateInvalidOperation,
        StateInvalidData,
        StateDuplicatedKeyError,
        StateEstablishmentClosed,
        StateEstablishmentFull,
        StateAppointmentCancellationError
    }
}
