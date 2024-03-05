namespace ExhibitsApplication.Models
{
    public class ExhibitsModel
    {
        public string InventoryNumber { get; set; }
        public string Name { get; set; }
        public string FundCode { get; set; }
        public string Year { get; set; }
        public string Quantity { get; set; }
        public string Material { get; set; }
        public string Size { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string RegistratonDate { get; set; }
        public byte[] Photo { get; set; }
    }
}
