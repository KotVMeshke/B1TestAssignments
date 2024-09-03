using Task1;

// Main program cycle of processing requests
var context = new ProgramContext();
while (true)
{
    context.DisplayMenu();
    context.HandleMenu();
}
