// Create Docking Relationships for Vessels stored in Neo4j

// Docking at Rotterdam
MATCH (p1:Port {name: 'Rotterdam'})
CREATE (p1)<-[:DOCKS_AT {vessel_name: 'Poseidon', arrival_date: date('2024-10-08'), departure_date: date('2024-10-12')}]-(),
       (p1)<-[:DOCKS_AT {vessel_name: 'Mariner', arrival_date: date('2024-10-18'), departure_date: date('2024-10-23')}]-(),
       (p1)<-[:DOCKS_AT {vessel_name: 'Horizon', arrival_date: date('2024-11-01'), departure_date: date('2024-11-05')}]-(),
       (p1)<-[:DOCKS_AT {vessel_name: 'Voyager', arrival_date: date('2024-11-10'), departure_date: date('2024-11-14')}]-();

// Docking at Hamburg
MATCH (p2:Port {name: 'Hamburg'})
CREATE (p2)<-[:DOCKS_AT {vessel_name: 'Titanic II', arrival_date: date('2024-10-06'), departure_date: date('2024-10-09')}]-(),
       (p2)<-[:DOCKS_AT {vessel_name: 'Enterprise', arrival_date: date('2024-10-20'), departure_date: date('2024-10-25')}]-(),
       (p2)<-[:DOCKS_AT {vessel_name: 'Catalyst', arrival_date: date('2024-11-03'), departure_date: date('2024-11-08')}]-(),
       (p2)<-[:DOCKS_AT {vessel_name: 'Discovery', arrival_date: date('2024-11-15'), departure_date: date('2024-11-19')}]-();

// Docking at Copenhagen
MATCH (p3:Port {name: 'Copenhagen'})
CREATE (p3)<-[:DOCKS_AT {vessel_name: 'Neptune', arrival_date: date('2024-11-01'), departure_date: date('2024-11-05')}]-(),
       (p3)<-[:DOCKS_AT {vessel_name: 'Liberty', arrival_date: date('2024-10-15'), departure_date: date('2024-10-20')}]-(),
       (p3)<-[:DOCKS_AT {vessel_name: 'Explorer II', arrival_date: date('2024-11-10'), departure_date: date('2024-11-15')}]-(),
       (p3)<-[:DOCKS_AT {vessel_name: 'Galileo', arrival_date: date('2024-11-20'), departure_date: date('2024-11-25')}]-();

// Docking at Oslo
MATCH (p4:Port {name: 'Oslo'})
CREATE (p4)<-[:DOCKS_AT {vessel_name: 'Endeavour', arrival_date: date('2024-10-15'), departure_date: date('2024-10-20')}]-(),
       (p4)<-[:DOCKS_AT {vessel_name: 'Pioneer', arrival_date: date('2024-11-05'), departure_date: date('2024-11-09')}]-(),
       (p4)<-[:DOCKS_AT {vessel_name: 'Odyssey', arrival_date: date('2024-11-18'), departure_date: date('2024-11-22')}]-(),
       (p4)<-[:DOCKS_AT {vessel_name: 'Voyager', arrival_date: date('2024-12-01'), departure_date: date('2024-12-05')}]-();

// Docking at Antwerp
MATCH (p6:Port {name: 'Antwerp'})
CREATE (p6)<-[:DOCKS_AT {vessel_name: 'Discovery', arrival_date: date('2024-11-02'), departure_date: date('2024-11-06')}]-(),
       (p6)<-[:DOCKS_AT {vessel_name: 'Neptune', arrival_date: date('2024-10-25'), departure_date: date('2024-10-30')}]-(),
       (p6)<-[:DOCKS_AT {vessel_name: 'Poseidon', arrival_date: date('2024-11-11'), departure_date: date('2024-11-16')}]-(),
       (p6)<-[:DOCKS_AT {vessel_name: 'Enterprise', arrival_date: date('2024-12-05'), departure_date: date('2024-12-10')}]-();

// Docking at Stockholm
MATCH (p7:Port {name: 'Stockholm'})
CREATE (p7)<-[:DOCKS_AT {vessel_name: 'Liberty', arrival_date: date('2024-10-09'), departure_date: date('2024-10-13')}]-(),
       (p7)<-[:DOCKS_AT {vessel_name: 'Horizon', arrival_date: date('2024-11-10'), departure_date: date('2024-11-15')}]-(),
       (p7)<-[:DOCKS_AT {vessel_name: 'Mariner', arrival_date: date('2024-11-20'), departure_date: date('2024-11-25')}]-(),
       (p7)<-[:DOCKS_AT {vessel_name: 'Odyssey', arrival_date: date('2024-12-05'), departure_date: date('2024-12-10')}]-();

// Docking at Amsterdam
MATCH (p5:Port {name: 'Amsterdam'})
CREATE (p5)<-[:DOCKS_AT {vessel_name: 'Odyssey', arrival_date: date('2024-10-20'), departure_date: date('2024-10-24')}]-(),
       (p5)<-[:DOCKS_AT {vessel_name: 'Voyager', arrival_date: date('2024-11-15'), departure_date: date('2024-11-20')}]-(),
       (p5)<-[:DOCKS_AT {vessel_name: 'Explorer II', arrival_date: date('2024-11-25'), departure_date: date('2024-11-30')}]-(),
       (p5)<-[:DOCKS_AT {vessel_name: 'Galileo', arrival_date: date('2024-12-01'), departure_date: date('2024-12-06')}]-();

// Docking at Marseille
MATCH (p10:Port {name: 'Marseille'})
CREATE (p10)<-[:DOCKS_AT {vessel_name: 'Enterprise', arrival_date: date('2024-11-10'), departure_date: date('2024-11-15')}]-(),
       (p10)<-[:DOCKS_AT {vessel_name: 'Pioneer', arrival_date: date('2024-12-05'), departure_date: date('2024-12-10')}]-(),
       (p10)<-[:DOCKS_AT {vessel_name: 'Catalyst', arrival_date: date('2024-12-11'), departure_date: date('2024-12-16')}]-(),
       (p10)<-[:DOCKS_AT {vessel_name: 'Discovery', arrival_date: date('2024-12-20'), departure_date: date('2024-12-24')}]-();
