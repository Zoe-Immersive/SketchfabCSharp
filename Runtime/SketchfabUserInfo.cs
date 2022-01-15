
using System.Collections.Generic;

public class SketchfabUserInfo
{

    private const int DEFAULT_REQ_PER_DAY = 75;

    private Dictionary<string, int> m_RequestsPerDay = new Dictionary<string, int> {
        { "basic", 75 },
        { "pro", 100 },
        { "prem", 200 },
    };

    public string Uid { get; private set; }

    public string Email { get; private set; }

    public string Account { get; private set; }

    public string Username { get; private set; }

    public int NumRequestsPerDay { get; private set; }

    public SketchfabUserInfo() { }

    public SketchfabUserInfo(string _uid, string _email, string _account, string _username )
    {
        Uid = _uid;
        Email = _email;
        Account = _account;
        Username = _username;
        NumRequestsPerDay = m_RequestsPerDay.ContainsKey(Account) ? m_RequestsPerDay[Account] : DEFAULT_REQ_PER_DAY;
    }

    public override string ToString()
    {
        return $"UID: {Uid}\n" +
                $"Email: {Email}\n" +
                $"Account: {Account}\n" +
                $"Username: {Username}\n" + 
                $"Requests Per Day: {NumRequestsPerDay}";
    }
}

