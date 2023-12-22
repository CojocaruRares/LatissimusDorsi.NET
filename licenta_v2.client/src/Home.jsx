import { useState, useEffect } from 'react';
import { auth } from './utils/firebase-config';

const Home = () => {
    const [userRole, setUserRole] = useState('');

    useEffect(() => {     
        const getUserRole = async () => {
            try {
                const currentUser = auth.currentUser;
                if (currentUser) {
                    const idTokenResult = await currentUser.getIdTokenResult();
                    setUserRole(idTokenResult.claims.role);
                }
            } catch (error) {
                console.log(error);
            }
        };
        getUserRole(); 
    }, []); 

    return (
        <div>
            <h2>Home</h2>
            <p>User Role: {userRole}</p>
        </div>
    );
};

export default Home;
