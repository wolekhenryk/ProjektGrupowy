import React, { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import httpClient from "../httpClient";
import "./css/ScientistProjects.css";

const SubjectVideoGroupAssignmentAdd = () => {
    const [formData, setFormData] = useState({
        subjectId: '',
        videoGroupId: ''
    });
    const [subjects, setSubjects] = useState([]);
    const [videoGroups, setVideoGroups] = useState([]);
    const [projectId, setProjectId] = useState(null);
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [dataLoading, setDataLoading] = useState(true);
    const navigate = useNavigate();
    const location = useLocation();

  useEffect(() => {
    const queryParams = new URLSearchParams(location.search);
    const projectIdParam = queryParams.get("projectId");
    if (projectIdParam) {
      setProjectId(parseInt(projectIdParam));
      fetchSubjectsAndVideoGroups(parseInt(projectIdParam));
    }
  }, [location.search]);

    const fetchSubjectsAndVideoGroups = async (projectId) => {
        setDataLoading(true);
        try {
            const [subjectsRes, videoGroupsRes] = await Promise.all([
                httpClient.get(`/project/${projectId}/subjects`),
                httpClient.get(`/project/${projectId}/videogroups`)
            ]);
            
            setSubjects(subjectsRes.data);
            setVideoGroups(videoGroupsRes.data);
        } catch (err) {
            setError(err.response?.data?.message || 'Failed to load data');
        } finally {
            setDataLoading(false);
        }
    };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError("");

    if (!formData.subjectId || !formData.videoGroupId) {
      setError("Please select both a subject and a video group.");
      setLoading(false);
      return;
    }

    try {
      await httpClient.post("/SubjectVideoGroupAssignment", {
        subjectId: parseInt(formData.subjectId),
        videoGroupId: parseInt(formData.videoGroupId),
      });
      navigate(`/projects/${projectId}`);
    } catch (err) {
      setError(
        err.response?.data?.message || "An error occurred. Please try again."
      );
      setLoading(false);
    }
  };

    if (dataLoading) {
        return (
            <div className="container d-flex justify-content-center align-items-center py-5">
                <div className="spinner-border text-primary" role="status">
                    <span className="visually-hidden">Loading...</span>
                </div>
            </div>
        );
    }

    return (
        <div className="container py-4">
            <div className="row justify-content-center">
                <div className="col-lg-8">
                    <div className="card shadow-sm">
                        <div className="card-header bg-primary text-white">
                            <h1 className="heading mb-0">Add New Assignment</h1>
                        </div>
                        <div className="card-body">
                            {error && <div className="alert alert-danger mb-4">{error}</div>}

                            <form onSubmit={handleSubmit}>
                                <div className="mb-3">
                                    <label htmlFor="subjectId" className="form-label">Subject</label>
                                    <select
                                        id="subjectId"
                                        name="subjectId"
                                        value={formData.subjectId}
                                        onChange={handleChange}
                                        className="form-select"
                                        required
                                    >
                                        <option value="">Select a subject</option>
                                        {subjects.map(subject => (
                                            <option key={subject.id} value={subject.id}>
                                                {subject.name}
                                            </option>
                                        ))}
                                    </select>
                                </div>

                                <div className="mb-4">
                                    <label htmlFor="videoGroupId" className="form-label">Video Group</label>
                                    <select
                                        id="videoGroupId"
                                        name="videoGroupId"
                                        value={formData.videoGroupId}
                                        onChange={handleChange}
                                        className="form-select"
                                        required
                                    >
                                        <option value="">Select a video group</option>
                                        {videoGroups.map(videoGroup => (
                                            <option key={videoGroup.id} value={videoGroup.id}>
                                                {videoGroup.name}
                                            </option>
                                        ))}
                                    </select>
                                </div>

                                <div className="d-flex">
                                    <button 
                                        type="submit" 
                                        className="btn btn-primary me-2"
                                        disabled={loading}
                                    >
                                        <i className="fas fa-plus-circle me-2"></i>
                                        {loading ? "Creating..." : "Create Assignment"}
                                    </button>
                                    <button 
                                        type="button" 
                                        className="btn btn-secondary"
                                        onClick={() => navigate(`/projects/${projectId}`)}
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

export default SubjectVideoGroupAssignmentAdd;
