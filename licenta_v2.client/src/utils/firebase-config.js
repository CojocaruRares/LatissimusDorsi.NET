// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
//import { getAnalytics } from "firebase/analytics";
import { getAuth } from 'firebase/auth';
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
    apiKey: "AIzaSyDzD3dQqaEaFcKYi22YJvXj3C-YH6Ae-aM",
    authDomain: "licenta-10cc3.firebaseapp.com",
    projectId: "licenta-10cc3",
    storageBucket: "licenta-10cc3.appspot.com",
    messagingSenderId: "763821289301",
    appId: "1:763821289301:web:afb9c5b627f0a2f75209dc",
    measurementId: "G-3HEETJY1GF"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const auth = getAuth(app);
export { app, auth };