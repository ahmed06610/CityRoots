namespace CityRoots.Core.DTOs.Schedule
{
    public class ScheduleDisplayDTO
    {
        public int ScheduleId {  get; set; }
        public int CycleId {  get; set; }
        public string TaskName {  get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string status {  get; set; }
        public string TaskDescription { get; set; }


    }
}
