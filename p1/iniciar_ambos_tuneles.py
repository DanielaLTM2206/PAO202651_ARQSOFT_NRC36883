import subprocess
import re
import time
import os
import sys

CLOUDFLARED = r"C:\Users\danie\Desktop\8vo\arqui\cloudflared.exe"
TXT_OUTPUT = os.path.join(os.path.dirname(os.path.abspath(__file__)), "ENLACES_DE_HOY.txt")

def start_tunnel(port):
    proc = subprocess.Popen(
        [CLOUDFLARED, "tunnel", "--url", f"http://127.0.0.1:{port}"],
        stderr=subprocess.PIPE,
        stdout=subprocess.PIPE,
        text=True,
        bufsize=1
    )
    url = None
    start = time.time()
    for line in proc.stderr:
        m = re.search(r"https://[a-zA-Z0-9-]+\.trycloudflare\.com", line)
        if m:
            url = m.group(0)
            break
        if time.time() - start > 25:
            break
    return proc, url

print("\n" + "="*70, flush=True)
print("     INICIANDO TUNELES CLOUDFLARE PARA AMBOS SERVIDORES", flush=True)
print("="*70, flush=True)

print("\n[1/2] Conectando Servidor SOAP Java (Puerto 8080)...", flush=True)
proc_soap, url_soap = start_tunnel(8080)
if url_soap:
    print(f"      [OK] SOAP URL: {url_soap}", flush=True)
else:
    print("      [AVISO] No se pudo obtener la URL de SOAP a tiempo.", flush=True)

print("\n[2/2] Conectando Servidor RESTful .NET (Puerto 5000)...", flush=True)
proc_rest, url_rest = start_tunnel(5000)
if url_rest:
    print(f"      [OK] REST URL: {url_rest}", flush=True)
else:
    print("      [AVISO] No se pudo obtener la URL de REST a tiempo.", flush=True)

# Guardar en archivo de texto para facil copia
with open(TXT_OUTPUT, "w", encoding="utf-8") as f:
    f.write("="*70 + "\n")
    f.write("          ENLACES DE HOY PARA LA PRESENTACION (GR08)\n")
    f.write("="*70 + "\n\n")
    f.write("--- 1. PROYECTO SOAP JAVA (Puerto 8080) ---\n")
    f.write(f"Dominio:     {url_soap}\n")
    f.write(f"Cliente Web: {url_soap}/ClienteWeb/\n")
    f.write(f"WSDL:        {url_soap}/WS_ConUni_Java_SOAP/WSConversorUnidades?wsdl\n\n")
    f.write("--- 2. PROYECTO RESTful .NET 8 (Puerto 5000) ---\n")
    f.write(f"Dominio:     {url_rest}\n")
    f.write(f"Cliente Web: {url_rest}/app/index.html\n")
    f.write(f"Swagger:     {url_rest}/\n\n")
    f.write("="*70 + "\n")

print("\n" + "="*70, flush=True)
print("              ¡TUS ENLACES ESTAN LISTOS PARA USAR!", flush=True)
print("="*70, flush=True)
print(f"\n  [SOAP Java 8080]:", flush=True)
print(f"   -> Copia para Consola/Escritorio: {url_soap}", flush=True)
print(f"   -> Para Navegador Web:           {url_soap}/ClienteWeb/", flush=True)
print(f"\n  [RESTful .NET 5000]:", flush=True)
print(f"   -> Copia para Consola/Escritorio: {url_rest}", flush=True)
print(f"   -> Para Navegador Web:           {url_rest}/app/index.html", flush=True)
print(f"   -> Documentacion Swagger:        {url_rest}/", flush=True)
print("\n" + "="*70, flush=True)
print(f"  Se guardo un bloc de notas con estos links en:", flush=True)
print(f"  {TXT_OUTPUT}", flush=True)
print("="*70, flush=True)
print("\nPresiona Ctrl + C o cierra esta ventana cuando termines de exponer.\n", flush=True)

try:
    while True:
        time.sleep(1)
except KeyboardInterrupt:
    print("\nCerrando tuneles...", flush=True)
    proc_soap.terminate()
    proc_rest.terminate()
    print("Tuneles cerrados con exito.", flush=True)
