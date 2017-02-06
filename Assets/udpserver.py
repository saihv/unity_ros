import socket
import time

UDP_IP = "127.0.0.1"
UDP_PORT = 5005
value = 1.11


print "UDP target IP:", UDP_IP
print "UDP target port:", UDP_PORT

sock = socket.socket(socket.AF_INET, # Internet
                     socket.SOCK_DGRAM) # UDP
while True:
    value = value + 0.01
    MESSAGE = "17.63, "+str(value)+", -23.42"
    print "message:", MESSAGE
    sock.sendto(MESSAGE, (UDP_IP, UDP_PORT))
    time.sleep(0.2)
