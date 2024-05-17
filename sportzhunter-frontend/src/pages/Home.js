import React from 'react';
import '../styles/home.css';
import Button from '../common/Button';
import { useNavigate } from 'react-router-dom';
import home1 from '../styles/images/home1.svg';

function Home() {
    const navigate = useNavigate();
    return(
        <center>  
            <div className="home">
                <h1>Welcome to SportzHunter</h1>
                <div className='intro_view'>
                    <div className='intro'>
                        Join us in the exhilarating world of sports excellence! <br/>
                        At SpotzHunter, we pride ourselves on fostering a dynamic <br/> community where passion meets performance. 
                        Whether you're an aspiring <br/> athlete, a seasoned pro, or simply a sports enthusiast, our organization <br/> offers an
                        unmatched platform for growth, camaraderie, and achievement.
                        <div className='home1'>
                            <img src={home1} alt='home1' style={{width:"50%"}}/>
                        </div>
                    </div>
                    <div className='intro'>
                        Come be a part of something truly special. 
                        Join us today <br/>and embark on an exhilarating journey filled with unforgettable <br/> moments,
                        lifelong friendships, and the pursuit of greatness. <br/>
                        Let's make every game, every match, and every victory count together!"
                    </div>
                </div>
                <Button onClick={() => navigate('/register')}>JOIN US</Button>
            </div>
        </center>
    );
}

export default Home;