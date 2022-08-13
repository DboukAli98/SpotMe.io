import axios from "axios";
import React, { useEffect, useState } from "react";
import { styled } from "@mui/material/styles";
import Card from "@mui/material/Card";
import CardHeader from "@mui/material/CardHeader";
import CardContent from "@mui/material/CardContent";
import CardActions from "@mui/material/CardActions";
import Collapse from "@mui/material/Collapse";
import Avatar from "@mui/material/Avatar";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import HighlightIcon from '@mui/icons-material/Highlight';
import ShareIcon from "@mui/icons-material/Share";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Loader from 'react-loader';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import Logo from "../assets/logo2.png";
import "../App.css";
import {Button} from "@mui/material";
import {useSelector } from "react-redux";





const ExpandMore = styled((props) => {
  const { expand, ...other } = props;
  return <IconButton {...other} />;
})(({ theme, expand }) => ({
  transform: !expand ? "rotate(0deg)" : "rotate(180deg)",
  marginLeft: "auto",
  transition: theme.transitions.create("transform", {
    duration: theme.transitions.duration.shortest,
  }),
}));



const JobsCards = () => {
  const [jobs, setJobs] = useState([]);

  const [open , setOpen] = useState(false);
  

  const [expanded, setExpanded] = useState(false);
  const [loaded, setLoaded] = useState(true);
  const [historyJobs , setHistoryJobs] = useState([]);
  

  const [data,setData] = useState('');


  const handleExpandClick = () => {
    setExpanded(!expanded);
  };
  const userId = useSelector((state) => state.id);

  const loadJobs = async (e) => {
    setLoaded(false);
    const response = await axios.get("https://localhost:7278/api/Jobs", {
      method: "GET",
      headers: {
        "Access-Control-Allow-Credentials": true,
        "Access-Control-Allow-Origin": "*",
        "Access-Control-Allow-Methods": "GET",
        "Access-Control-Allow-Headers": "application/json",
      },
    });

    if (response.status === 200) {
      const data = await response.data;
      setJobs(data);
      setLoaded(true);
    } else {
      console.log("Error fetching jobs " + response.statusText);
      setLoaded(true);
    }
  };

  const loadHistory = async () => {
    const response = await axios.get(`https://localhost:7278/api/JobApplication/GetJobApp?appId=${userId}`,{
      method: "GET",
      headers: {
        "Access-Control-Allow-Credentials": true,
        "Access-Control-Allow-Origin": "*",
        "Access-Control-Allow-Methods": "GET",
        "Access-Control-Allow-Headers": "application/json",
      },

    });

    if(response.status === 200){
      const data = await response.data;
      setHistoryJobs(data);
      
      
     
      
    }else{
      console.log("Error" + response.statusText);
    }
  }

  useEffect(() => {
    loadJobs();
    loadHistory();
  }, [userId]);

  const handleOpen = async (data) => {
    setOpen(true);
    setData(data);
    
    
  };
  const handleClose = () => setOpen(false);

  

 

  

  

  
  const jobIdApp =  data.jobId;
  
  

  const handleOnClose = () => {
    setOpen(false);
  };
  
  const [fileSelected, setFileSelected] = useState();

  const saveFileSelected= (e) => {
    //in case you wan to print the file selected
    console.log(e.target.files[0]);
    setFileSelected(e.target.files[0]);
  };

  
  
  


  const applyToJob = async () => {
    const formData = new FormData();
    formData.append("pdfFile", fileSelected);
    formData.append("fileName",'');
    formData.append("cvFile",'');
    const response = await axios.post(`https://localhost:7278/api/JobApplication/CreateEnterprise?appId=${userId}&jobId=${jobIdApp}`,formData);
    setLoaded(false);
    if(response.status === 201){
      setLoaded(true);
      window.location.reload(true);

      
    }else{  
      setLoaded(true);
    }
  };
  
  
  
 

  return (
    
    <div  className="job-card">
      <Loader
        loaded={loaded}
        lines={13}
        length={20}
        width={10}
        radius={30}
        corners={1}
        rotate={0}
        direction={1}
        color="blue"
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
      
      
      <Box sx={{ flexGrow: 1 }}>
        <Grid
          container
          spacing={{ xs: 2, md: 4 }}
          columns={{ xs: 1, sm: 8, md: 12 }}
        >
          {jobs.map((item ,i) => (
            <Grid item key={i} xs={2} sm={4} md={4}>
              <Card key={i} sx={{ maxWidth: 350 }}>
                <CardHeader
                  avatar={
                    <Avatar alt={item.enterprise.enterpriseName} src={item.enterprise.logo ? item.enterprise.logo : Logo  } 
                    sx={{ width: 66, height: 66 }}   />
                  }
                  action={
                    <IconButton   aria-label="settings">
                      <MoreVertIcon  />
                    </IconButton>
                  }
                  title={item.jobTitle + item.jobId}
                  subheader={item.jobLocation}
                />
                <CardContent>
                  <Typography  variant="body2" color="text.secondary">
                    {item.jobDescription}
                  </Typography>
                </CardContent>
                <CardActions   disableSpacing>
                  <IconButton  key={item.jobId} disabled={ userId && historyJobs.some(index => index.jobId === item.jobId)}   onClick={async ()  => await handleOpen(item)}  aria-label="apply to job">
                    <HighlightIcon/>{item.jobId}
                  </IconButton>
                  <IconButton  aria-label="share">
                    <ShareIcon  />
                  </IconButton>
                  <ExpandMore   
                    expand={expanded}
                    onClick={handleExpandClick}
                    aria-expanded={expanded}
                    aria-label="show more"
                  >
                    <ExpandMoreIcon />
                  </ExpandMore>
                </CardActions>
                <Collapse in={expanded} timeout="auto" unmountOnExit>
                  <CardContent >
                    <Typography   paragraph>Method:</Typography>
                    <Typography  paragraph>
                      {item.enterprise.enterpriseName}
                    </Typography>
                    <Typography  paragraph></Typography>
                    <Typography  paragraph>{item.Requirements}</Typography>
                    <Typography >{item.jobDomain}</Typography>
                    <Typography >{item.applicants.length} Applicants</Typography>
                  </CardContent>
                </Collapse>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Box>
     { data && <Dialog open={open} onClose={handleClose}>
        <DialogTitle>Subscribe</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Apply to {data.jobTitle} at {data.enterprise.enterpriseName}
          </DialogContentText>
          <input type="file" onChange={saveFileSelected} />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleOnClose}>Cancel</Button>
          <Button onClick={applyToJob}>Apply</Button>
        </DialogActions>
      </Dialog>
     }
   
    </div>
  );
};

export default JobsCards;

