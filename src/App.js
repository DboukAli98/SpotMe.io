import './App.css';
import {
  Routes,
  Route,
  Link,
} from 'react-router-dom';
import Home from './components/Home';
import Login from './components/Login';
import Register from './components/Register';
import ResponsiveAppBar from './components/AppBar';
import {useSelector } from 'react-redux';
import Dashboard from './components/Dashboard';
import Enterprise from './components/Enterprise';
import Applications from './components/Applications';
import Chat from './components/Chat';
import Profile from './components/Profile';
import React from 'react';
import HomeRecruiter from './components/recruiter_screens/recruiter_homescreen';
import SplashScreen from './components/splashscreen';








function App() {

  const roles = {recruiter :'Recruiter' , applicant :'User', admin :'Admin'};

  const logged = useSelector((state) => state.isLoggedIn);
  const role = useSelector((state) => state.role);

  
  


  return (
    
    <div className="App">
      {logged && <ResponsiveAppBar/>}
      <Routes>
        {logged && role ===roles.applicant && <Route path='/' element={<Home/>}  />}
        {logged && role === roles.recruiter && <Route path='/recruiterhome' element={<HomeRecruiter/>}/>}
        <Route path='/login' element={<Login/>}/>
        <Route path='/splash' element={<SplashScreen/>}/>
        <Route path='/register' element={<Register/>}/>
        <Route path='/about' element={<About/>}/>
        <Route path='/Dashboard' element={<Dashboard/>}/>
        <Route path='/Enterprise' element={<Enterprise/>}/>
        <Route path='/applications' element={<Applications/>}/>
        <Route path='/profile' element={<Profile/>}/>
        <Route path='/chat' element={<Chat/>}/>
      </Routes>
      
    </div>
  );
}

const About = () => {
  return (
    <>
    <main>
      <h2>Who we are ?</h2>
      <p>
          That feels like an existential question, don't you
          think?
        </p>
    </main>
    <nav>
    <Link to="/">Home</Link>
  </nav>
  </>
  )

};

export default App;
