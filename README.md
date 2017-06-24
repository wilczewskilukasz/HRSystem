# HRInfo System
## Multiplatform mobile app based on Ionic framework designed for viewing and managing employees personal data.

To test the client locally:

1. Install Ionic framework: [Getting started guide](https://ionicframework.com/getting-started/)

2. Clone or download the repository

3. Navigate to the direcotry of your choice and type the following in the cmd to create new Ionic project:
```
ionic start HRInfo tabs
```
3. Navigate to the previously created project folder:
```
cd HRInfo
```
4. Add missing call-number plugin dependencies:
```
ionic cordova plugin add call-number
(choose 'Y'es when prompted to install cli-plugin-cordova plugin)
npm install --save @ionic-native/call-number
```
5. Replace src folder with the one from the downloaded repository (HRSystem/HRInfo/src).

6. Type following to run app in your default browser:
```
ionic serve --lab
```
