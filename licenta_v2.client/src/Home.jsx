import { useState, useEffect } from 'react';
import { auth } from './utils/firebase-config';
import './Home.css';
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
            <div className="hero-image">
                <div className="hero-text">
                    <h1>Train like a beast</h1>
                    <p>Fitness made simple</p>                
                </div>
            </div>

            <h2 className='title'>What Athletes Are Saying</h2>

            <div className="athlete-comments-container">
               
                <div className="athlete-comment"> 
                    <img src="../public/img/home/trainer.jpg" className="user-image" alt="img" />
                    <div>
                        <p className="comment-text">&ldquo; LattisimusDorsi.net enables me to craft customizable training programs for users effortlessly.
                        Its intuitive interface simplifies communication, ensuring seamless interaction with clients.
                            &rdquo;</p>
                        <br></br>
                        <p className="user-type">John, weightlifting trainer.</p>
                    </div>
                </div>

                <div className="athlete-comment">
                    <img src="../public/img/home/user1.jpeg" className="user-image" alt="img" />
                    <div>
                        <p className="comment-text">&ldquo; I&apos;ve tried many workout apps, but none compare to LattisimusDorsi.net.
                            The personalized programs are top-notch, and the community aspect adds a
                            motivating factor that keeps me coming back for more.
                            &rdquo;</p>
                        <br></br>
                        <p className="user-type">Jim, basic user.</p>
                    </div>
                </div>

                <div className="athlete-comment">
                    <img src="../public/img/home/user2.jpg" className="user-image" alt="img" />
                    <div>
                        <p className="comment-text">&ldquo; Finding the right workout routine can be daunting,
                            but LattisimusDorsi.net takes the guesswork out of it.
                            The user-friendly interface makes it easy to navigate, and the results speak for themselves.
                            &rdquo;</p>
                        <br></br>
                        <p className="user-type">Miguel, basic user.</p>
                    </div>
                </div>

                <div className="athlete-comment">
                    <img src="../public/img/home/user3.jpg" className="user-image" alt="img"/>
                    <div>
                        <p className="comment-text">&ldquo; As a professional athlete, I rely on LattisimusDorsi.net to keep me in peak condition.
                            The trainers behind the platform are experts in their field,
                            and their guidance has helped me achieve my fitness goals faster than I ever thought possible.
                            &rdquo;</p>
                        <br></br>
                        <p className="user-type">Marry, basic user.</p>
                    </div>
                </div>
            </div>
            <div className="d-flex justify-content-center">
            <div className='plan-workout'>
                    <div className='workout-text'>
                        <h2>Get the perfect training program.</h2>
                        <br />
                        <p>With the ability to customize factors like RPE (Rate of Perceived Exertion) and reps,
                            trainers can fine-tune workouts to optimize effectiveness and cater to individual fitness levels.</p>
                        <br/>
                        <p>It&apos;s not just about providing workouts, it&apos;s about delivering precisely what each user requires to thrive on their fitness journey.</p>
                        <br />
                        <p>Whether it&apos;s delivered via email for convenient access on-the-go or accessed directly on our platform,
                            users can effortlessly stay on track with their fitness journey.</p>
                </div>
                <img src="../public/img/home/workout.png" className='workout-image' alt="Workout" />
                </div>
            </div>


        </div>
    );
};

export default Home;