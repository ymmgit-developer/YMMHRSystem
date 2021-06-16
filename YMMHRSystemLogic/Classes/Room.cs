using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMMHRSystemLogic;

namespace YMMHRSystemLogic
{
    public class Room
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the Room DTO
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns>DtoRoom Loaded</returns>
        public DtoRoom Load(long roomId)
        {
            DtoRoom room = new DtoRoom();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (roomId != 0)
                {
                    mapping.Load<DtoRoom>("Rooms", new DtoRoom(), "RoomId=" + roomId);
                    room = (DtoRoom)mapping.dtoList.FirstOrDefault().Dto;
                }

                return room;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Room", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register Room 
        /// </summary>
        /// <param name="room"></param>
        /// <returns>Room registered</returns>
        public void Save(DtoRoom room)
        {
            try
            {
                User user = new User();

                if (room.RoomId == 0)
                {
                    room.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = room, TableName = "Rooms" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Room", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Room with fields.
        /// </summary>
        /// <returns>Load Room Dto</returns>
        public List<DtoRoom> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoRoom> roomList = new List<DtoRoom>();

                mapping.Load<DtoRoom>("SELECT RoomId, Name, UserCreated, DateAdded FROM Rooms ORDER BY RoomId", "Room", new DtoRoom());
                roomList.AddRange(mapping.dtoList.Select(renglon => (DtoRoom)renglon.Dto));

                return roomList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Room", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get Room Id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public long GetRoomId(string name)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT RoomId FROM Rooms WHERE Name = '" + name + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get RoomId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["RoomId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Room Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetRoomId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Room. 
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public bool DeleteRoom(long roomId)
        {
            try
            {
                string query = "DELETE Rooms WHERE RoomId =" + roomId;
                oDatabase.ExecuteNonQuery(query, "Remove Room");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Room", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteRoom");
                return false;
            }

        }
        #endregion
    }
}
