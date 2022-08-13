import { createSlice } from "@reduxjs/toolkit";

const authSlice = createSlice({
    name: 'auth',
    initialState:{isLoggedIn:false , username:'',role:'',id:''},
    reducers:{
        login(state,action){
            state.isLoggedIn = true;
            state.username = action.payload;
        },
        setRole(state,action){
            state.role = action.payload;
        },
        setUserId(state,action){
            state.id = action.payload;
        },
        setLogout(state){
            state.isLoggedIn = false;
            state.id='';
            state.role='';
            state.username='';
        },
        
    }
});

export const authActions = authSlice.actions;
export default authSlice;