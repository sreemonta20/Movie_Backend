using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movie.BackEnd.Common.Utilities.Interfaces
{
    public interface IMessageResposeState
    {
        void SetMsgResState(out string message, out bool state, Enum eventoperation);
        //void SetMsgResSate<T>(out string message, out bool resstate, Enum eventoperation, T singleobject, List<T> collection) where T : class;
        //void SetResStateForEntity<T>(out string message, out bool resstate, ref T singleobject, Enum eventoperation) where T : class;
        //void SetMsgResStateForCollection<T>(out string message, out bool resstate, ref List<T> singleobject, Enum eventoperation) where T : class;
    }
}
