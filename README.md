#CAPPamari

CAPPamari is a drag-and-drop scheduling tool designed to aid RPI students in fulfilling their CAPP report requirements. A live version of the site can be found [here]( https://cappamari.iecfusor.com/).

##Installation

###Requirements

* IIS
* .NET 4.0 (or greater)
* MSSQL database

###Procedure

* Download the [pre-compiled site image](https://github.com/ngpitt/CAPPamari/blob/master/CAPPamari.zip)
* Extract the image to your web server
* Use SQL Server Management Studio or a similar application to create CAPPamari’s database by navigating to the Models folder and executing CAPPamari.edmx.sql
* Use SQL Server Management Studio to create a user with read/write access to the newly created database
* Open ConnectionStrings.config and replace the placeholder credentials with the username and password of the new database user
* Use IIS Manager to add site with a .NET 4.0 application pool whose physical path points to the location of the extracted site image
