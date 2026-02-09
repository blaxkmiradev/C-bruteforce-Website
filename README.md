# K-SEC Bruteforcer

Simple, fast, no-bullshit C# console tool for checking combo lists against login forms.  
Auto-detects common fields (email/username + password/pass/pwd), saves hits in timestamped folders.

**Currently tuned for:** https://auth-kris.vercel.app/login (redirects to /dashboard on success)

![K-SEC Banner](https://via.placeholder.com/728x90/000000/FF0000?text=K-SEC+BRUTEFORCER)  
*(replace with real screenshot or ASCII render if you want)*

## Features
- Only asks for combo file path (full path or relative)
- Hardcoded target + success check (easy to change in code)
- Creates folder like `auth-kris.vercel.app_2025-02-09_16-47-22`
- Saves valid logins to `success.txt` inside that folder
- Basic anti-ban delay (380-450ms)
- Ignores errors silently, keeps running
- Looks aggressive in console with colors + ASCII

## Usage

1. Compile the .cs file (Visual Studio / dotnet CLI / whatever)
   ```bash
   csc Program.cs
   # or
   dotnet new console -o KSecBrute
   # paste code into Program.cs, then dotnet run

Put your combos in a .txt file (format: email:password or user:pass one per line)
Run the exetextTarget login URL is hardcoded → https://auth-kris.vercel.app/login
Success if redirect contains → dashboard

Combo file path → C:\tool\found.txt   (or just found.txt if same folder)
Press ENTER → it starts cooking

Hits saved automatically in:
textauth-acg,com.app_YYYY-MM-DD_HH-mm-ss/
└── success.txt
Example output
text[K-SEC BRUTEFORCER • ach,cim]
═══════════════════════════════════════════════════════

Combo file path → C:/combos.txt

Target:      ach,com/login
Success str:  dashboard
Combos:       1337
Hits save:    ach.app_2025-02-09_17-12-45/success.txt

ENTER to start...

[1/1337] test@email.com                     ... 
[HIT #1] test@email.com:pass123 → https://ach/dashboard?session=abc
Customization (edit in code)
Change these lines if you want different target:
C#string loginUrl = "https:/achcom.com/login";
string successContains = "dashboard";
If site uses different fields (not email/password):
C#new KeyValuePair<string, string>("email", email),
new KeyValuePair<string, string>("password", pass)
Warnings / Heads up

This is for educational / testing purposes only (wink)
Most sites ban IPs fast — add proxies if you go hard
Captcha / rate-limit / JS challenges = dead
No threading yet (single-thread slow but stealthy)

Todo / Next shit

Proxy list support
Multi-threading
JSON login support
Custom success check (body contains / status code / etc)
Bigger ASCII banner options

Fuck around & find out
Drop issues or PRs if you add dope features.
K-SEC was here. Stay savage.
