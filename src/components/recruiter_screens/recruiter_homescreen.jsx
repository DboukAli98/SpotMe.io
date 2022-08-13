import React from 'react'
import RecruitersJobsCards from './recruiters_jobs';

const HomeRecruiter = () => {
  return (
    <div>HomeRecruiter
      <RecruitersJobsCards/>
    </div>
  )
}

export default HomeRecruiter;



{/* <form onSubmit={createJobPost}>
      <input type="text" placeholder='job title' onChange={(e) => setTitle(e.target.value)}></input>
      <input type="text" placeholder='description' onChange={(e) => setDescription(e.target.value)}></input>
      <input type="text" placeholder='location' onChange={(e) => setLocation(e.target.value)}></input>
      <input type="text" placeholder='type' onChange={(e) => setType(e.target.value)}></input>
      <input type="text" placeholder='job domain' onChange={(e) => setDomain(e.target.value)}></input>
      <input type="text" placeholder='requirements' onChange={(e) => setRequirements(e.target.value)}></input>
      <button type='submit'>Post this job !</button>
</form> */}

// const [title , setTitle] = useState();
//   const [description , setDescription] = useState();
//   const [location , setLocation] = useState();
//   const [type , setType]=useState();
//   const [domain , setDomain] = useState();
//   const [requirements , setRequirements] = useState();

// const createJobPost = async (e) => {
//     e.preventDefault();
//     const jsonData = {JobTitle: title , JobDescription : description , JobLocation: location , EmploymentType : type , JobDomain: domain , Requirements : requirements};
//     const post = await fetch(`https://localhost:7013/api/Jobs/CreateJobPost?Id=${id}`,{
//       method:'POST',
//       headers: {
//         'Content-Type': 'application/json'
//       },
//       body : JSON.stringify(jsonData)
//     });
//     if(post.status === 200){
//       console.log("Success Job Posted" + post.ok);
//     }else {
//       console.log("Error" + post.statusText);
//     }

//   };