namespace Task2.Excel.DTO
{
    public class ExcelFileDTO
    {
        public string BankName { get; set; } = "";
        public List<ExcelBankClassDTO> BankClasses { get; set; } = new List<ExcelBankClassDTO>();
        public ExcelRowDTO FileSum { get; set; } = new ExcelRowDTO();
    }
}
