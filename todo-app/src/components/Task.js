import { FaPencilAlt, FaTimes, FaPlayCircle } from 'react-icons/fa';
import "../index.css"

const Task = ({ task, onDelete, onEdit, onPlay }) => {
    return (
        <div>
            <div className="task">
                <div style={{display: 'flex'}}>
                    {task && task.audio && (
                    <div style={{minWidth: 30}}>
                        <p>
                            <FaPlayCircle className="playIcon" onClick={() => onPlay(task)} />
                        </p>
                    </div>
                    )}
                    <div>
                        <p className="taskName">
                            <span className="textBold">What:</span> {task.name}
                        </p>
                        <p className="taskDate"><span className="textBold">When:</span> {task.day}</p>
                    </div>
                </div>
                <div>
                    <p><FaTimes onClick={() => onDelete(task.id)} className="delIcon" /></p>
                    <p><FaPencilAlt onClick={() => onEdit(task.id)} className="editIcon" /></p>
                </div>
            </div>
        </div>
    )
}

export default Task
