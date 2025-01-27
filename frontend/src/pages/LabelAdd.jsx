import React, { useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import './css/ScientistProjects.css';

const AddLabel = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const [labelData, setLabelData] = useState({
        name: '',
        colorHex: '#ffffff',  // Default color
        type: 'range',
        shortcut: '',
        subjectId: new URLSearchParams(location.search).get('subjectId'), // Get subjectId from query params
    });
    const [error, setError] = useState('');  // To store error message

    // Handle form input change
    const handleChange = (e) => {
        const { name, value } = e.target;
        setLabelData((prevState) => ({
            ...prevState,
            [name]: value,
        }));
    };

    // Validate the form data
    const validateForm = () => {
        if (labelData.shortcut.length !== 1) {
            setError('Shortcut must be exactly one character.');
            return false;
        }
        if (!/^#[0-9A-Fa-f]{6}$/.test(labelData.colorHex)) {
            setError('Color must be in the format #ffffff.');
            return false;
        }
        setError('');
        return true;
    };

    // Handle form submission
    const handleSubmit = async (e) => {
        e.preventDefault();

        // Validate before submitting
        if (!validateForm()) {
            return;
        }

        try {
            const response = await fetch(`http://localhost:5000/api/Label`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(labelData),
            });

            if (response.ok) {
                alert('Label added successfully');
                navigate(`/subjects/${labelData.subjectId}`); // Redirect to the subject details page
            } else {
                alert('Failed to add label');
            }
        } catch (error) {
            console.error('Error adding label:', error);
        }
    };

    return (
        <div className="container">
            <h1>Add Label</h1>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="name">Name</label>
                    <input
                        type="text"
                        id="name"
                        name="name"
                        value={labelData.name}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="colorHex">Color</label>
                    <input
                        type="color"
                        id="colorHex"
                        name="colorHex"
                        value={labelData.colorHex}
                        onChange={handleChange}
                    />
                </div>
                {/*<div>*/}
                {/*    <label htmlFor="type">Type</label>*/}
                {/*    <input*/}
                {/*        type="text"*/}
                {/*        id="type"*/}
                {/*        name="type"*/}
                {/*        value={labelData.type}*/}
                {/*        onChange={handleChange}*/}
                {/*        required*/}
                {/*    />*/}
                {/*</div>*/}
                <div>
                    <label htmlFor="shortcut">Shortcut</label>
                    <input
                        type="text"
                        id="shortcut"
                        name="shortcut"
                        value={labelData.shortcut}
                        onChange={handleChange}
                        maxLength="1" // Limit to one character
                        required
                    />
                </div>
                {error && <p style={{ color: 'red' }}>{error}</p>} {/* Display error message */}
                <button type="submit">Add Label</button>
            </form>
            <button
                className="btn-back"
                onClick={() => navigate(`/subjects/details/${labelData.subjectId}`)}
            >
                Back to Subject Details
            </button>
        </div>
    );
};

export default AddLabel;
