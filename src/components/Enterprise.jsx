import axios from "axios";
import React, { useEffect, useState } from "react";
import Card from "@mui/material/Card";
import CardHeader from "@mui/material/CardHeader";
import CardContent from "@mui/material/CardContent";
import Avatar from "@mui/material/Avatar";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Logo from "../assets/logo2.png";

const Enterprise = () => {
  const [enterprise, setEnterprise] = useState([]);

  const fetchEnterprises = async () => {
    const response = axios.get(
      "https://localhost:7278/api/Enterprise/Enterprises",
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
    if ((await response).status === 200) {
      const data = (await response).data;
      setEnterprise(data);
      console.log(enterprise);
    } else {
      console.log("Error");
    }
  };

  useEffect(() => {
    fetchEnterprises();
  }, []);

  return (
    <>
      <h1>Enterprise</h1>

      <>
        <Box sx={{ flexGrow: 1 }}>
          <Grid
            container
            spacing={{ xs: 2, md: 4 }}
            columns={{ xs: 1, sm: 8, md: 12 }}
          >
            {enterprise.map((item, i) => (
              <Grid item key={i} xs={2} sm={4} md={4}>
                <Card key={i} sx={{ maxWidth: 350 }}>
                  <CardHeader
                    avatar={
                      <Avatar
                        alt={item.enterpriseName}
                        src={item.logo ? item.logo : Logo}
                        sx={{ width: 66, height: 66 }}
                      />
                    }
                    title={item.enterpriseName}
                    
                  />
                  <CardContent>
                    <Typography variant="body2" color="text.secondary">
                      {item.enterpriseDescription}
                    </Typography>
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>
        </Box>
      </>
    </>
  );
};

export default Enterprise;
