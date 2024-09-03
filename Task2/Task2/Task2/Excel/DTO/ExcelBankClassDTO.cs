namespace Task2.Excel.DTO
{
    public class ExcelBankClassDTO
    {
        public string ClassName { get; set; } = "";
        public List<ExcelRowDTO> Rows { get; set; } = new List<ExcelRowDTO>();
        public ExcelRowDTO ClassSum { get; set; } = new ExcelRowDTO();
    }
}
