import { useEffect, useState } from "react";
import userService from "../../services/UserService";
import Button from "../../common/Button";
import { useNavigate } from "react-router-dom";
import '../../styles/tournament.css';

export default function RegisterForm() {
    const [countyList, setCountyList] = useState([]);
    const [positionList, setPositionList] = useState([]);
    const [sportCategoryList, setSportCategoryList] = useState([]);
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
      FirstName: '',
      LastName: '',
      Username: '',
      Password: '',
      VerifyPassword: '',
      Email: '',
      SportCategory: '',
      TeamId: null,
      Height: '',
      Weight: '',
      PreferredPositionId: '',
      DateOfBirth: '',
      CountyId: '',
      Description: '',
    });

    const getCounties = async () => {
        await userService.getCounties().then((response) => {
          setCountyList(response.data);
          setFormData((prevData) => ({ ...prevData, CountyId: response.data[0].id }));
          });
    }
    const getPrefPositions = async () => {
        await userService.getPrefPositions().then((response) => {
          setPositionList(response.data);
          setFormData((prevData) => ({ ...prevData, PreferredPositionId: response.data[0].id }));
        });
    }
    const getSportCategories = async () => {
        await userService.getSportCategories().then((response) => {
          setSportCategoryList(response.data);
          setFormData((prevData) => ({ ...prevData, SportCategory: response.data[0].id }));
        });
    }

    useEffect(() => {
      const fetchData = async () => {
      await getCounties();
      await getPrefPositions();
      await getSportCategories();
    };

    fetchData();
    }, []);


    const handleChange = (e) => {
        setFormData({...formData, [e.target.name]: e.target.value});
    }

    const isValidEmail = (email) => {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
      };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (formData.Password !== formData.VerifyPassword)
        {
          setError("Passwords do not match");
          return; 
        }

        if(isValidEmail(formData.Email) === false)
        {
          setError("Invalid email");
          return;
        }

        if(formData.Weight < 0)
        {
          setError("Weight cannot be negative");
          return;
        }

        if(formData.Height < 0)
        {
          setError("Height cannot be negative");
          return;
        }

        try
        {
          const response = await userService.register(formData);
          if(response.status === 200) {
            navigate("/login");
          }
        }catch  (error) {
          setError(error.response.data);
        }
    }
    
    return (
      <div className="formDiv">
      <h1>Register</h1>
        <form onSubmit={handleSubmit} className="form">
        {error && <p>{error}</p>}
        <label>First Name:</label>
          <input type="text" name="FirstName" value={formData.FirstName} onChange={handleChange} required/>
        <br />
  
        <label>Last Name: </label>
          <input type="text" name="LastName" value={formData.LastName} onChange={handleChange} required/>
        <br />
  
        <label>Username:</label>
          <input type="text" name="Username" value={formData.Username} onChange={handleChange} required/>       
        <br />
  
        <label>Password:</label>
          <input type="password" name="Password" value={formData.Password} onChange={handleChange} required/>   
        <br />
  
        <label>Verify Password: </label>
          <input type="password" name="VerifyPassword" value={formData.VerifyPassword} onChange={handleChange} required/> 
        <br />
  
        <label>Email:</label>
          <input type="text" name="Email" value={formData.Email} onChange={handleChange} required/>
        <br />
  
        <label>Sport Category:</label>
          <select name="SportCategory" value={formData.SportCategory} onChange={handleChange} required>
            {sportCategoryList.map((category) => <option key={category.id} value={category.id}>{category.categoryName}</option>)}
          </select>
        <br />
  
        <label>County:</label>
          <select name="CountyId" value={formData.CountyId} onChange={handleChange} required>
            {countyList.map((county) => <option key={county.id} value={county.id}>{county.countyName}</option>)}
          </select>
        <br />
  
        <label>Preferred Position:</label>
          <select name="PreferredPositionId" value={formData.PreferredPositionId} onChange={handleChange} required>
            {positionList.map((position) => <option key={position.id} value={position.id}>{position.positionName}</option>)}
          </select>  
        <br />
  
        <label>Date of Birth:</label>
          <input type="date" name="DateOfBirth" value={formData.DateOfBirth} onChange={handleChange} required/>       
        <br />

        <label>Weight:</label>
          <input type="number" name="Weight" value={formData.Weight} onChange={handleChange} required/>
        <br />

        <label>Height:</label>
          <input type="number" name="Height" value={formData.Height} onChange={handleChange} required/>
        <br />
  
        <label>Description:</label>
          <textarea name="Description" value={formData.Description} onChange={handleChange} required/>
        <br />
  
        <Button>Register</Button>
      </form>
      </div>
    );
}