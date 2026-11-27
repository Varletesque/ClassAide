namespace ClassAideApp.Model;

public record Course(String Code, String Name, int ID = 0)
{
    public override string ToString() => $"{Code} - {Name}";
}
