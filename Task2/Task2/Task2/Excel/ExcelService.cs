using Microsoft.EntityFrameworkCore;
using Task2.DataBase;
using Task2.DataBase.Entity;
using Task2.Excel.DTO;
using Task2.Excel.ExcelData;

namespace Task2.Excel
{
    /// <summary>
    /// Service class for handling operations related to Excel files,
    /// including importing Excel data into the database and retrieving files.
    /// </summary>
    /// <param name="db">Database context for interaction with database</param>
    /// <param name="logger">Logger</param>
    public class ExcelService(ApplicationDBContext db, ILogger<ExcelService> logger)
    {
        /// <summary>
        /// Imports data from an uploaded Excel file into the database.
        /// </summary>
        /// <param name="fileXLS">The uploaded Excel file.</param>
        /// <returns>A boolean indicating whether the import was successful.</returns>
        public async Task<bool> ImportIntoDB(IFormFile fileXLS)
        {
            try
            {
                // Parsing an Excel file with a .XLS extension 
                var parsedFile = ExcelParser.ParseXLS(fileXLS.OpenReadStream());

                // Create a new file instance and inporting it into the database
                var file = new DataBase.Entity.File()
                {
                    Name = fileXLS.FileName
                };

                await db.Files.AddAsync(file);

                // Obtaining bank enity, if it doesn't exist create a new one and add it into database 
                var bank = await db.Banks
                    .FirstOrDefaultAsync(b => b.Name == parsedFile.BankName);
                if (bank == null)
                {
                    bank = new Bank() { Name = parsedFile!.BankName };
                    await db.Banks.AddAsync(bank);
                }

                // Look through each bank class parsed from Excel file
                foreach (var bankCl in parsedFile.BankClasses)
                {
                    // Obtaining bank class enity, if it doesn't exist create a new one and add it into database 
                    var bankClass = await db.BankClasses
                        .FirstOrDefaultAsync(b => b.Name == bankCl.ClassName);
                    if (bankClass is null)
                    {
                        bankClass = new BankClass() { Name = bankCl.ClassName };
                        await db.BankClasses.AddAsync(bankClass);
                    }

                    var rowList = new List<BankAccount>();

                    // Loop through each row in the current bank class and create new BankAccount entity
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
                            File = file,
                            IsSum = row.IsSum,
                        });
                    }

                    // Add all rows into database
                    await db.BankAccounts.AddRangeAsync(rowList);
                }

                // Savev database changes and inform user about it
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
        /// <summary>
        /// Obtains a list of all uploaded Excel files from the database.
        /// </summary>
        /// <returns>A list of <see cref="FileDTO"/> representing the files, or null if an error occurs.</returns>
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
        /// <summary>
        /// Obtains data for a specific Excel file by its ID, including its associated bank accounts and classes.
        /// </summary>
        /// <param name="fileId">The ID of the file to obtain.</param>
        /// <returns>An <see cref="ExcelFileDTO"/> containing the file data, or null if an error occurs.</returns>
        public async Task<ExcelFileDTO?> GetFile(int fileId)
        {
            try
            {
                // Obtaining data from database with save file data and with data grouping
                var fileData = await db.BankAccounts
                    .Where(b => b.FileId == fileId)
                    .Include(b => b.BankClass)
                    .Include(b => b.Bank)
                    .GroupBy(b => b.BankClass!.Name)
                    .ToListAsync();

                if (fileData is null) return null;

                // Creating new instance of ExcelFileDTO to send it to the user
                var excelFile = new ExcelFileDTO()
                {
                    BankName = fileData.FirstOrDefault()!.FirstOrDefault()!.Bank!.Name,
                };
                
                // Loop through each bank classe in the obtained data
                foreach (var bankClass in fileData)
                {
                    // Creat new instance of ExcelBankClassDTO
                    var newBankClass = new ExcelBankClassDTO()
                    {
                        ClassName = bankClass.Key
                    };
                  

                    // Loop thourgh each rows in the current bank class and creat DTOs of each
                    foreach (var item in bankClass)
                    {
                        var currentRow = new ExcelRowDTO()
                        {
                            AccountNumber = item.AccountNumber,
                            ClosingBalanceActive = item.ClosingBalanceActive,
                            ClosingBalancePassive = item.ClosingBalancePassive,
                            OpeningBalanceActive = item.OpeningBalanceActive,
                            OpeningBalancePassive = item.OpeningBalancePassive,
                            TurnoverCredit = item.TurnoverCredit,
                            TurnoverDebit = item.TurnoverDebit,
                            IsSum = item.IsSum,
                        };

                        // Handl different kinds of rows
                        if (currentRow.AccountNumber.Contains("ПО КЛАССУ", StringComparison.OrdinalIgnoreCase))
                            newBankClass.ClassSum = currentRow;
                        else if (currentRow.AccountNumber.Contains("БАЛАНС", StringComparison.OrdinalIgnoreCase))
                        {
                            excelFile.FileSum = currentRow;
                        }else
                        {
                            newBankClass.Rows.Add(currentRow);
                        }



                    }

                    // Add completed bank calss into result class
                    excelFile.BankClasses.Add(newBankClass);
                }
                return excelFile;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception was occured during file getting with message: {ex}", ex.Message);
                return null;
            }
        }
    }
}
