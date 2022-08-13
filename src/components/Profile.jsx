import axios from "axios";
import React from "react";
import { useEffect } from "react";
import { useState } from "react";
import { useSelector } from "react-redux";
import Avatar from "@mui/material/Avatar";
import Loader from "react-loader";
import Stack from "@mui/material/Stack";
import Box from "@mui/material/Box";
import { Typography } from "@mui/material";
import '../App.css';

const Profile = () => {
  const [info, setInfo] = useState();
  const [loaded, setLoaded] = useState(true);

  const userId = useSelector((state) => state.id);
  const username = useSelector((state) => state.username);

  const loadUser = async () => {
    setLoaded(false);
    const response = await axios.get(
      `https://localhost:7278/api/Authentication/GetApplicant?userId=${userId}`,
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

    if ((await response.status) === 200) {
      const data = await response.data;
      setInfo(data);
      setLoaded(true);
      console.log(data);
    } else {
      console.log("Error " + response.statusText);
      setLoaded(true);
    }
  };

  useEffect(() => {
    loadUser();
  }, [userId]);

  return (
    <div>
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
      <h1>{username} Profile</h1>
      {info && <Box sx={{ width: "100%" }}>
        <Stack spacing={2}>
          
            <div className="avatar-profile">
            <Avatar
              alt={info.profileName}
              src={info.logo}
              sx={{ width: 300, height: 300 }}
            ></Avatar>
            </div>
          
          <Typography>{info.user.firstname}  {info.user.lastname}</Typography>
          <Typography>{info.skills}</Typography>
          <Typography>{info.user.email}</Typography>
        </Stack>
      </Box>}
    </div>
  );
};

export default Profile;
