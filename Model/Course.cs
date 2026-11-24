namespace ClassAideApp.Model;

public record Course(int id, String Code, String Name)
{
    public override string ToString() => $"{Code} - {Name}";
}
