# Ed-Fi.Credentials Sample

This is a stripped down version of the application which the Wisconsin Department of Public Instruction uses to manage credentials for its statewide Ed-Fi implementation.  In Ed-Fi.Credentials, WI DPI staff are the primary users, however each district is required to log in to authorize their vendors to submit data for their district.  This is intended to be self-service application for districts, though occasionally situations arise which require DPI staff to adjust credentials on behalf of a district.  Additionally, vendors can log in to retrieve the credentials issued to them and view the access level of any credential issued to them.  As much as has been reasonable, functionality which is specific to WI DPI has been removed in this version, but customizations which were more broadly application remain.

A note on authentication and authorization: the State of Wisconsin has a statewide authentication system (WAMS) which is accessible to anyone in the state including districts and vendors and the Wisconsin Department of Public Instruction has a separate authorization system (ASM/WISEsecure) which provides authorization for its applications.  These two systems are integral to the functioning of the Ed-Fi.Credentials application, but are not included in this sample.  Those components have been removed and default values are inserted to allow it to function in demo form.  


## Getting Started

### Database
An EdFi_Admin, EdFi_Security, and ODS database are a prerequisite for this application.  Scripts are located in the Database folder to add the additional objects needed for those databases.  This sample project references a generic version of EdFi_ODS, though it would normally reference a multi-year ODS.  Please adjust the following views to suit your implementation:
	Agency and AgencySchool in the ODS database for Education Organization data
	LatestEdOrgs in EdFi_Admin for Education Organization data
	AsmUser in EdFi_Admin for WAMS comparable user data
Scripts are located in the Database folder to add the additional objects needed for those databases.

### Code
Configure the connectionStrings in the web.config in the Ed-Fi.Credential.MVC project.

Urls for services and external sites are located in appSettings section in the web.config in the Ed-Fi.Credential.MVC project.  These services are not critical for the functioning of the site in demo form.

The ImplementationSpecific folder in the in the Ed-Fi.Credential.MVC project contains the bulk of the dummy code for WI DPI integration points for this project.



## Copyright
Copyright 2020 Wisconsin Department of Public Instruction.

## License
Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and limitations under the License.
