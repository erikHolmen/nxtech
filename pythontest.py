a = []
p = 0
import datetime
import json
import requests
import time


with open('temperature.txt', 'r') as f:
    for line in f:
        a.append(int(line.strip()))

def getTemperature():
    global p
    v = a[p]
    time.sleep(0.01)
    p+=1
    if p>=len(a):
        p=0
    v = (v*100./4095) - 50
    return v


def getMeasurement():
    temps = []
    tstart = time.time()
    for _ in range(1200):
        temps.append(getTemperature())
    tend = time.time()
    return {
        "time":{
            "start": time2iso(tstart),
            "end": time2iso(tend)
        },
        "min": "%.2f" % min(temps),
        "max": "%.2f" % max(temps),
        #avg istedenfor average
        "avg": "%.2f" % (sum(temps)/len(temps))
    }
    
def time2iso(t):
    t = int(t)
    return datetime.datetime.utcfromtimestamp(t).isoformat() + "Z"


def post(obj, url):
    try:
        r = requests.post(url, json=obj, timeout=0.5) #timeout kan endres til noe passende
        if r.status_code == 500:
            return False
        if r.status_code == 200:
            return True    
        print(r.text)
        return False
    except requests.exceptions.ConnectionError:
        return False

url1 = "http://localhost:5000/api/temperature"
url2 = "http://localhost:5000/api/temperature/missing"

fail1 = None
missing = []

while True:
    m = getMeasurement()
    if len(missing):
        if len(missing) > 10:
            print("dropping %d" % (len(missing)-10))
            missing = missing[-10:]
        print("missing")
        if post(missing, url2):
            missing = []
    if fail1:
        print("fail1")
        if not post(fail1, url1):
            missing.append(fail1)
        fail1 = None
    if not post(m, url1):
        fail1 = m
