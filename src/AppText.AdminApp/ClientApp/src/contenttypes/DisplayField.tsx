import React from 'react';
import { FaFont, FaCalculator, FaAlignLeft, FaCalendarAlt, FaPen } from 'react-icons/fa';

import { Field as ContentTypeField } from './models';

interface DisplayFieldProps {
  field: ContentTypeField,
  onSelect: () => void
}

const iconMap = {
  'ShortText' : FaFont,
  'LongText' : FaAlignLeft, 
  'DateTime' : FaCalendarAlt,
  'Number' : FaCalculator
};

const DisplayField: React.FunctionComponent<DisplayFieldProps> = ({ field, onSelect }) => {
  
  const Icon = iconMap[field.fieldType];

  return (
    <div className="card">
      <div className="card-body">
        <div className="float-right">
          <button className="btn btn-link" onClick={onSelect}>
            <FaPen />
          </button>
        </div>
        <div>
          <Icon className="mr-2" />
          <strong>{field.name}</strong>
        </div>
        <div>{field.description}</div>
      </div>
    </div>
  );
};

export default DisplayField;