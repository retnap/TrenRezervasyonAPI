namespace TrenRezervasyonAPI.Models
{
    public class TrenModel
    {
        public string Ad { get; set; } = string.Empty;
        public List<VagonModel> Vagonlar { get; set; } = new List<VagonModel>();
    }
}