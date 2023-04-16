import glob
import os
import re
import requests
import sys

SUNSHINE_USER = ""
SUNSHINE_PASSWORD = ""


desktop_path = os.path.join(
    os.path.abspath(os.environ.get('HOMEPATH')),
    "Desktop"
)

start_menu_path = os.path.join(
    os.path.abspath(os.environ.get('HOMEPATH')),
    "Desktop"
)


os.chdir(desktop_path)
filelist = glob.glob('**\\*.url', recursive=True)

template = "C:\\Games\\SunshineGameLauncher.exe \"%s\" %s"
grabber = re.compile('URL=(.*)\n.*?IconFile=(.*)[$|\n]')
exefind = re.compile('.*\\\(.*?)\.exe')
namefind = re.compile('\\\(.*?)\.url')


r = requests.get("https://localhost:47990/api/apps", auth=('sunshine', 'open'), verify=False)
existing_apps = r.json()["apps"]
cmds = [a['cmd'] for a in existing_apps if 'cmd' in a.keys()]


def add_app(name, link, exe):
    payload = {
        "name": name,
        "output":"",
        "cmd": template % (link, exe),
        "index": -1,
        "exclude-global-prep-cmd": False,
        "prep-cmd":[],
        "detached":[],
        "image-path":""
    }
    url = "https://localhost:47990/api/apps"

    if payload['cmd'] in cmds:
        return

    for c in cmds:
        if c.startswith(payload['cmd']):
            return

    print ("Adding game: %s" % name)
    r = requests.post(url, auth=('sunshine', 'open'), json=payload, verify=False)
    if r.status_code != 200:
        print ("Error: " + r.content)


def main():
    for filename in filelist:
        filename = '.\\' + filename
        r = namefind.findall(filename)
        if len(r) == 0:
            print ("ERROR: There should be a name from the file!")

        name = r[0]
        with open(filename) as f:
            data = f.read()
            results = grabber.findall(data)
            if len(results) > 0:
                (link, icon) = results[0]
                r = exefind.findall(icon)
                if len(r) > 0:
                    exe = r[0]
                else:
                    exe = ""
                if link.startswith("com.epicgames.launcher://") or \
                   link.startswith("steam://"):
                    add_app(name, link, exe)

if __name__ == '__main__':
    main()
