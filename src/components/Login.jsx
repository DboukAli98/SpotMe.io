import React, { useState} from 'react';
import { useDispatch } from 'react-redux';
import {  useNavigate } from 'react-router-dom';
import {authActions} from '../store/auth-slice';
//Material UI for UI components
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import Loader from 'react-loader';
import Alert from '@mui/material/Alert';


function Copyright(props) {
  return (
    <Typography variant="body2" color="text.secondary" align="center" {...props}>
      {'Copyright © '}
      <Link color="inherit" href="https://mui.com/">
        SpotMe
      </Link>{' '}
      {new Date().getFullYear()}
      {'.'}
    </Typography>
  );
}


const theme = createTheme();

const Login = () => {
  

 const dispatch = useDispatch();

  const [email , setEmail] = useState('');
  const [password , setPassword] = useState('');
  const [error , setError] = useState(false);
  const [loaded, setLoaded] = useState(true);
  const [errorMessage , setErroMessage] = useState('');
  
  
 
  const navigate = useNavigate();

  const  loginHandle = async (e) => {
    e.preventDefault();
    setLoaded(false);

    const user = {email , password};
    const log  =  await fetch("https://localhost:7278/api/Authentication/Login" , {
      method : 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body : JSON.stringify(user),
      
      
    });
    

    if(log.status === 200) {
      const data = await log.json();
      console.log(data.id);
      setLoaded(true);
      const username = data.user;
      const role = data.role;
      const id = data.id;

      
      dispatch(authActions.login(username));
      dispatch(authActions.setRole(role));
      dispatch(authActions.setUserId(id));
      
      if(data.role==="Recruiter"){
      navigate("/recruiterhome", {replace:true});
      }else if(data.role === "User"){
        navigate("/" , {replace:true});
      }else if (data.role ==="Admn"){
        navigate("/splash" , {replace:true});
      }else{
        navigate("/splash" , {replace:true});
      }

    }else {
      setLoaded(true);
      setError(true);
      setErroMessage("Login Failed " + log.statusText);
      

    }


  };

  return (
    <ThemeProvider theme={theme}>
      {error &&<Alert severity="error">{errorMessage}</Alert>}
       <Loader
        loaded={loaded}
        lines={13}
        length={20}
        width={10}
        radius={30}
        corners={1}
        rotate={0}
        direction={1}
        color="#000"
        speed={1}
        trail={60}
        shadow={false}
        hwaccel={false}
        className="spinner"
        zIndex={2e9}
        top="50%"
        left="50%"
        scale={1.0}
        loadedClassName="loadedContent"
      />
    <Container component="main" maxWidth="xs">
      <CssBaseline />
      <Box
        sx={{
          marginTop: 8,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
        }}
      >
        <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
          
        </Avatar>
        <Typography component="h1" variant="h5">
          Log In
        </Typography>
        <Box component="form" onSubmit={loginHandle} noValidate sx={{ mt: 1 }}>
          <TextField
            margin="normal"
            required
            fullWidth
            id="email"
            label="Email Address"
            onChange={(e) =>setEmail(e.target.value)}
            name="email"
            autoComplete="email"
            autoFocus
          />
          <TextField
            margin="normal"
            required
            fullWidth
            name="password"
            label="Password"
            onChange={(e) => setPassword(e.target.value)}
            type="password"
            id="password"
            autoComplete="current-password"
          />
          <FormControlLabel
            control={<Checkbox value="remember" color="primary" />}
            label="Remember me"
          />
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            Sign In
          </Button>
          <Grid container>
            <Grid item xs>
              <Link href="#" variant="body2">
                Forgot password?
              </Link>
            </Grid>
            <Grid item>
              <Link href="/register" variant="body2">
                {"Don't have an account? Sign Up"}
              </Link>
            </Grid>
          </Grid>
        </Box>
      </Box>
      <Copyright sx={{ mt: 8, mb: 4 }} />
    </Container>
  </ThemeProvider>
  );
}

export default Login