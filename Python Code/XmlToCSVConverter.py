from flask import Flask
from flask import request
from flask_restful import Api, Resource, reqparse
import xml.etree.ElementTree as ET
import csv
from xml.etree.ElementTree import XML, fromstring, tostring
import pika


app = Flask(__name__)
api = Api(app)

connection = pika.BlockingConnection(pika.ConnectionParameters('localhost'))
channel = connection.channel()


channel.queue_declare(queue='XMLToCSV')

def callback(ch, method, properties, body):

    output = body.decode('utf-8').split(":::")
    
    root = ET.fromstring(output[1])
    f = open(output[0]+".csv", 'w')
    csvwriter = csv.writer(f)

    count = 0

    head = ['MeasurementUnit','processingDateTime','softwareCreator','softwareName','softwareVersion']

    csvwriter.writerow(head)

    row = []

    mu = root.find('MeasurementUnit').text
    row.append(mu)

    processingDateTime = root.find('processingDateTime').text
    row.append(processingDateTime)

    softwareCreator = root.find('softwareCreator').text
    row.append(softwareCreator)

    softwareName = root.find('softwareName').text
    row.append(softwareName)

    softwareVersion = root.find('softwareVersion').text
    row.append(softwareVersion)

    csvwriter.writerow(row)
    f.close()
   
    

channel.basic_consume(callback,
                      queue='XMLToCSV',
                      no_ack=True)


channel.start_consuming()

