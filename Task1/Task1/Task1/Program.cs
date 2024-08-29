using Task1;

var context = new ProgramContext();
while (true)
{
    context.DisplayMenu();
    context.HandleMenu(out int numberOfOption);
}
