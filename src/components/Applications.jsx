import axios from 'axios';
import React, { useEffect, useState } from 'react'
import { useSelector } from 'react-redux';

const Applications = () => {
    const [data ,setData] = useState([]);
    const appId = useSelector((state) => state.id);

    const getApplications = async () => {
        const response = await axios.get(`https://localhost:7278/api/JobApplication/GetJobApp?appId=${appId}`);
        if(response.status === 200){
            console.log(response);
            setData(response.data);
        }else{
            console.log("Error " + response.statusText);
        }
    }
    useEffect(() => {
      getApplications();
    }, [])
    
  return (
    <div>
        <h1>
            Applications
        </h1>
        {data && data.map((item)=>
        <h2>Job Title : {item.jobTitle}</h2>
        )}


    </div>
  )
}

export default Applications