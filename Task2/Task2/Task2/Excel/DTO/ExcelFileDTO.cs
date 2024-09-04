namespace Task2.Excel.DTO
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing a excel file.
    /// </summary>
    public class ExcelFileDTO
    {
        public string BankName { get; set; } = "";
        public List<ExcelBankClassDTO> BankClasses { get; set; } = new List<ExcelBankClassDTO>();
        public ExcelRowDTO FileSum { get; set; } = new ExcelRowDTO();
    }
}
