using Microsoft.EntityFrameworkCore;
using Task2.DataBase;
using Task2.DataBase.Entity;
using Task2.Excel.DTO;
using Task2.Excel.ExcelData;

namespace Task2.Excel
{
    public class ExcelService(ApplicationDBContext db, ILogger<ExcelService> logger)
    {
        public async Task<bool> ImportIntoDB(IFormFile fileXLS)
        {
            try
            {
                var parsedFile = ExcelParser.ParseXLS(fileXLS.OpenReadStream());
                var file = new DataBase.Entity.File()
                {
                    Name = fileXLS.FileName
                };

                await db.Files.AddAsync(file);

                var bank = await db.Banks
                    .FirstOrDefaultAsync(b => b.Name == parsedFile.BankName);
                if (bank == null)
                {
                    bank = new Bank() { Name = parsedFile!.BankName };
                    await db.Banks.AddAsync(bank);
                }
                foreach (var bankCl in parsedFile.BankClasses)
                {
                    var bankClass = await db.BankClasses
                        .FirstOrDefaultAsync(b => b.Name == bankCl.ClassName);
                    if (bankClass is null)
                    {
                        bankClass = new BankClass() { Name = bankCl.ClassName };
                        await db.BankClasses.AddAsync(bankClass);
                    }

                    var rowList = new List<BankAccount>();
                    foreach (var row in bankCl.BankRows)
                    {
                        rowList.Add(new BankAccount()
                        {
                            AccountNumber = row.AccountNumber,
                            Bank = bank,
                            BankClass = bankClass,
                            ClosingBalanceActive = row.ClosingBalanceActive,
                            ClosingBalancePassive = row.ClosingBalancePassive,
                            OpeningBalanceActive = row.OpeningBalanceActive,
                            OpeningBalancePassive = row.OpeningBalancePassive,
                            TurnoverCredit = row.TurnoverCredit,
                            TurnoverDebit = row.TurnoverDebit,
                            File = file
                        });
                    }

                    await db.BankAccounts.AddRangeAsync(rowList);
                }

                await db.SaveChangesAsync();

                logger.LogInformation($"File was loaded succesfuly");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during file parsing with message: {ex}", ex.Message);
                return false;   
            }
        }

        public async Task<List<FileDTO>?> GetFiles()
        {
            try
            {
                var files = await db.Files
                    .Select(f => new FileDTO()
                    {
                        Id = f.Id,
                        Name = f.Name,
                    })
                    .ToListAsync();
                logger.LogInformation("Files were found succesfuly. Number of files: {qunitity}", files.Count);
                return files;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception was occured during files getting with message: {ex}", ex.Message);
                return null;
            }
        }

        public void GetFile(int fileId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                logger.LogError("Exception was occured during file getting with message: {ex}", ex.Message);
            }
        }
    }
}
