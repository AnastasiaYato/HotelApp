using Newtonsoft.Json;

namespace Service_HotelManagement.Helpers
{
    /// <summary>
    /// Makes it a bit easier to read in swagger. Also provides more readable functionality.
    /// </summary>
    public class StringPair
    {
        public string Reason { get; set; }
        public string Details { get; set; }
        public StringPair()
        {
            Reason = string.Empty;
            Details = string.Empty;
        }
        public StringPair(string reason, string details="")
        {
            Reason = reason;
            Details = details;
        }
        public override string ToString() {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
