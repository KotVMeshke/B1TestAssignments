namespace Task2.Excel.DTO
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing a bank class in Excel file.
    /// </summary>
    public class ExcelBankClassDTO
    {
        public string ClassName { get; set; } = "";
        public List<ExcelRowDTO> Rows { get; set; } = new List<ExcelRowDTO>();
        public ExcelRowDTO ClassSum { get; set; } = new ExcelRowDTO();
    }
}
