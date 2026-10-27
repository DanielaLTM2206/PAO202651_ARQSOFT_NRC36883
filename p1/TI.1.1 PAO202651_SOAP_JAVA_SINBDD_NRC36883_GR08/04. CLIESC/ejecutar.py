import os, subprocess
m2 = os.path.expanduser('~/.m2/repository')
dep_prefixes = [
    'jakarta/xml/ws/jakarta.xml.ws-api/4.0.1',
    'jakarta/xml/bind/jakarta.xml.bind-api/4.0.',
    'jakarta/xml/soap/jakarta.xml.soap-api/3.0.',
    'jakarta/activation/jakarta.activation-api/2.1.',
    'com/sun/xml/ws/jaxws-rt/4.0.2',
    'com/sun/xml/ws/policy/4.0.',
    'org/glassfish/gmbal/gmbal/4.0.',
    'org/glassfish/gmbal/gmbal-api-only',
    'org/glassfish/external/management-api',
    'org/glassfish/ha/ha-api',
    'org/jvnet/staxex/stax-ex',
    'com/sun/xml/stream/buffer/streambuffer',
    'org/jvnet/mimepull/mimepull',
    'com/sun/xml/fastinfoset/FastInfoset',
    'jakarta/mail/jakarta.mail-api',
    'org/eclipse/angus/angus-activation',
    'org/eclipse/angus/angus-mail',
    'com/sun/xml/messaging/saaj/saaj-impl',
    'com/fasterxml/woodstox/woodstox-core',
    'org/codehaus/woodstox/stax2-api',
    'org/glassfish/jaxb/jaxb-runtime/4.0.',
    'org/glassfish/jaxb/jaxb-core/4.0.',
    'com/sun/xml/bind/jaxb-impl/4.0.',
    'com/sun/xml/bind/jaxb-core/4.0.'
]
here = os.path.dirname(os.path.abspath(__file__))
jar = os.path.join(here, 'ClienteEscritorio', 'target', 'ClienteEscritorio-1.0-SNAPSHOT.jar')
jars = [jar]
for root, dirs, files in os.walk(m2):
    for f in files:
        if f.endswith('.jar') and not f.endswith('-sources.jar') and not f.endswith('-javadoc.jar'):
            full = os.path.join(root, f).replace('\\', '/')
            for dp in dep_prefixes:
                if dp in full:
                    jars.append(os.path.join(root, f))
                    break
cp = ';'.join(jars)
cmd = ['java', '-cp', cp, 'ec.edu.monster.clienteescritorio.ClienteEscritorio']
subprocess.run(cmd)
