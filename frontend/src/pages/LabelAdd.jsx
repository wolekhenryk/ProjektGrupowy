import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import httpClient from '../httpClient';
import './css/ScientistProjects.css';

const LabelAdd = () => {
    const [formData, setFormData] = useState({
        name: '',
        colorHex: '#3498db',
        type: 'range',
        shortcut: '',
        subjectId: null
    });
    const [subjectName, setSubjectName] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        // Extract subjectId from query params
        const queryParams = new URLSearchParams(location.search);
        const subjectId = queryParams.get('subjectId');
        
        if (!subjectId) {
            setError('Subject ID is required. Please go back and try again.');
            return;
        }
        
        setFormData(prev => ({ ...prev, subjectId: parseInt(subjectId) }));
        fetchSubjectName(parseInt(subjectId));
    }, [location.search]);

    const fetchSubjectName = async (id) => {
        try {
            const response = await httpClient.get(`/subject/${id}`);
            setSubjectName(response.data.name);
        } catch (err) {
            setError('Failed to load subject information.');
        }
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError('');

        if (!formData.name || !formData.colorHex || !formData.type || !formData.subjectId) {
            setError('Please fill in all required fields.');
            setLoading(false);
            return;
        }

        try {
            await httpClient.post('/label', formData);
            navigate(`/subjects/${formData.subjectId}`);
        } catch (err) {
            setError(err.response?.data?.message || 'An error occurred. Please try again.');
        } finally {
            setLoading(false);
        }
    };

    if (!formData.subjectId) {
        return (
            <div className="container py-4">
                <div className="alert alert-danger">
                    <i className="fas fa-exclamation-triangle me-2"></i>
                    {error || 'Missing Subject ID parameter'}
                </div>
                <button 
                    className="btn btn-secondary"
                    onClick={() => navigate('/projects')}
                    style={{height: 'fit-content', margin: '1%'}}
                >
                    <i className="fas fa-arrow-left me-2"></i>Back to Projects
                </button>
            </div>
        );
    }

    return (
        <div className="container py-4">
            <div className="row justify-content-center">
                <div className="col-lg-8">
                    <div className="card shadow-sm">
                        <div className="card-header bg-primary text-white">
                            <h1 className="heading mb-0">Add New Label</h1>
                        </div>
                        <div className="card-body">
                            {error && (
                                <div className="alert alert-danger mb-4">
                                    <i className="fas fa-exclamation-triangle me-2"></i>
                                    {error}
                                </div>
                            )}
                            
                            <form onSubmit={handleSubmit}>
                                <div className="mb-3">
                                    <label htmlFor="name" className="form-label">Label Name</label>
                                    <input
                                        type="text"
                                        id="name"
                                        name="name"
                                        value={formData.name}
                                        onChange={handleChange}
                                        className="form-control"
                                        required
                                        disabled={loading}
                                    />
                                </div>

                                <div className="mb-3">
                                    <label htmlFor="colorHex" className="form-label">Label Color</label>
                                    <div className="input-group">
                                        <input
                                            type="color"
                                            id="colorHex"
                                            name="colorHex"
                                            value={formData.colorHex}
                                            onChange={handleChange}
                                            className="form-control form-control-color"
                                            title="Choose label color"
                                            style={{ maxWidth: '60px' }}
                                            disabled={loading}
                                        />
                                        <input
                                            type="text"
                                            className="form-control form-control-color"
                                            value={formData.colorHex}
                                            onChange={(e) => setFormData({...formData, colorHex: e.target.value})}
                                            pattern="#[0-9A-Fa-f]{6}"
                                            placeholder="#RRGGBB"
                                            disabled={loading}
                                        />
                                        <span className="input-group-text" style={{ backgroundColor: formData.colorHex, width: '40px' }}></span>
                                    </div>
                                    <div className="form-text">Color in hexadecimal format (#RRGGBB)</div>
                                </div>

                                <div className="mb-3">
                                    <label htmlFor="type" className="form-label">Label Type</label>
                                    <select
                                        id="type"
                                        name="type"
                                        value={formData.type}
                                        onChange={handleChange}
                                        className="form-select"
                                        required
                                        disabled={loading}
                                    >
                                        <option value="range">Range</option>
                                        <option value="point">Point</option>
                                    </select>
                                </div>

                                <div className="mb-3">
                                    <label htmlFor="shortcut" className="form-label">Shortcut Key</label>
                                    <input
                                        type="text"
                                        id="shortcut"
                                        name="shortcut"
                                        value={formData.shortcut}
                                        onChange={handleChange}
                                        className="form-control"
                                        maxLength="1"
                                        placeholder="Single character (a-z, 0-9)"
                                        disabled={loading}
                                    />
                                    <div className="form-text">Single character used as keyboard shortcut for this label</div>
                                </div>

                                <div className="mb-4">
                                    <label htmlFor="subjectId" className="form-label">Subject</label>
                                    <div className="input-group">
                                        <span className="input-group-text">
                                            <i className="fas fa-folder"></i>
                                        </span>
                                        <input
                                            type="text"
                                            className="form-control"
                                            value={subjectName || `Subject ID: ${formData.subjectId}`}
                                            disabled
                                        />
                                    </div>
                                </div>

                                <div className="d-flex">
                                    <button 
                                        type="submit" 
                                        className="btn btn-primary me-2"
                                        disabled={loading}
                                    >
                                        {loading ? (
                                            <>
                                                <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                                                Adding...
                                            </>
                                        ) : (
                                            <>
                                                <i className="fas fa-plus-circle me-2"></i>Add Label
                                            </>
                                        )}
                                    </button>
                                    <button 
                                        type="button" 
                                        className="btn btn-secondary"
                                        onClick={() => navigate(`/subjects/${formData.subjectId}`)}
                                        disabled={loading}
                                    >
                                        <i className="fas fa-times me-2"></i>Cancel
                                    </button>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default LabelAdd;