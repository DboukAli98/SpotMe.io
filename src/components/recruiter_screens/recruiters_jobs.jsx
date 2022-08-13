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
import HighlightIcon from "@mui/icons-material/Highlight";
import ShareIcon from "@mui/icons-material/Share";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Loader from "react-loader";
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import {Button} from "@mui/material";
import Logo from "../../assets/logo2.png";
import "../../App.css";
import { useSelector } from "react-redux";

const RecruitersJobsCards = () => {
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

  const [jobs, setJobs] = useState([]);
  const [open, setOpen] = useState(false);

  const [expanded, setExpanded] = useState(false);
  const [loaded, setLoaded] = useState(true);
  const [data,setData] = useState('');

  const userId = useSelector((state) => state.id);
  console.log(userId);

  const handleExpandClick = () => {
    setExpanded(!expanded);
  };

  const handleOpen = async (data) => {
    setOpen(true);
    setData(data);
    console.log(data);
  };

  const loadJobs = async (e) => {
    setLoaded(false);
    const response = await axios.get(
      `https://localhost:7278/api/Jobs/GetRecruitersJobs?userId=${userId}`,
      {
        method: "GET",
        headers: {
          "Access-Control-Allow-Credentials": true,
          "Access-Control-Allow-Origin": "*",
          "Access-Control-Allow-Methods": "GET",
          "Access-Control-Allow-Headers": "application/json",
        },
      }
    );

    if (response.status === 200) {
      const data = await response.data;
      setJobs(data);
      setLoaded(true);
      console.log(data);
    } else {
      console.log("Error fetching jobs " + response.statusText);
      setLoaded(true);
    }
  };

  useEffect(() => {
    loadJobs();
  }, [userId]);

  return (
    <div className="job-card">
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
          {jobs.map((item, i) => (
            <Grid item key={i} xs={2} sm={4} md={4}>
              <Card key={i} sx={{ maxWidth: 350 }}>
                <CardHeader
                  avatar={
                    <Avatar
                      alt={item.enterprise.enterpriseName}
                      src={item.enterprise.logo ? item.enterprise.logo : Logo}
                      sx={{ width: 66, height: 66 }}
                    />
                  }
                  action={
                    <IconButton aria-label="settings">
                      <MoreVertIcon />
                    </IconButton>
                  }
                  title={item.jobTitle + item.jobId}
                  subheader={item.jobLocation}
                />
                <CardContent>
                  <Typography variant="body2" color="text.secondary">
                    {item.jobDescription}
                    Applicants <br></br>
                    {item.applicants.map((app) => app.user.firstname)}

                  </Typography>
                </CardContent>
                <CardActions disableSpacing>
                <IconButton  key={item.jobId}  onClick={async ()  => await handleOpen(item)}  aria-label="apply to job">
                    <HighlightIcon/>
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
                  <CardContent>
                    <Typography paragraph>Method:</Typography>
                    <Typography paragraph>
                      {item.enterprise.enterpriseName}
                    </Typography>
                    <Typography paragraph></Typography>
                    <Typography paragraph>{item.Requirements}</Typography>
                    <Typography>{item.jobDomain}</Typography>
                    <Typography>{item.applicants.length} Applicants</Typography>
                  </CardContent>
                </Collapse>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Box>
      { data && <Dialog open={open}>
        <DialogTitle>Subscribe</DialogTitle>
        <DialogContent>
          <DialogContentText>
            {jobs.map((item) => 
            <Typography>{item.applicants.user}</Typography>)}
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          
        </DialogActions>
      </Dialog>
     }
    </div>
  );
};

export default RecruitersJobsCards;
