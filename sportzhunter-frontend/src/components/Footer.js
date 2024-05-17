import React from 'react';
import { Link } from 'react-router-dom';

function Footer() {
  return (
    <footer>
      <div>
        <nav>
            <Link to="/">CONTACT US</Link>
            <Link to="/">ABOUT US</Link>
        </nav> 
        <p style={{color:"#042f54"}}>&copy; 2024 SportzHunter Website. All rights reserved.</p>
      </div>
    </footer>
  );
}

export default Footer;