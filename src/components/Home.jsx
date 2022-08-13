import React from "react";
import { useSelector } from "react-redux";
import { Link } from "react-router-dom";
import JobsCards from "./JobsCards";

const Home = () => {
  const user = useSelector((state) => state.username);

  return (
    <>
      <main>
        <h2>Welcome to the homepage! </h2>
        <p>You can do this, I believe in you.{user}</p>
      </main>

      {/* {role===roles.recruiter && enterprise.map((item => 
      <>
      <h1 key={item.jobId}>{item.jobTitle}</h1>
      <h2>{item.enterprise.enterpriseName}</h2>
      <h3>{item.applicants.map(i => <p>{i.id}</p>)}</h3>
      </>
      ))} */}

      <JobsCards />

      {/* {enterprise.enterpriseName} */}

      <nav>
        <Link to="/about">About</Link>
        <br></br>
        <Link to="/login">Login</Link>
      </nav>
    </>
  );
};

export default Home;
