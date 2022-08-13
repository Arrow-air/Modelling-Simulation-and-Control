clear;
clc;

pkg load mapping;

adb = importdata('mga_v2_adb.dat');
l_elevon = importdata('l_elevon.dat');
r_elevon = importdata('r_elevon.dat');
rudder = importdata('rudder.dat');

list = {'CFX','CFY','CFZ','CMX','CMY','CMZ'};

output_selection = {'CFZ'};

beta = 0; % in degrees
alpha = 0; % in degrees
deflection = 0; % in degrees

%{
adeflection = roundn(adeflection/2  * 180/pi,0)*2; % in degrees
if adeflection > 20
  adeflection = 20;
end
if adeflection < -20
  adeflection = -20;
end
edeflection = roundn(edeflection/2  * 180/pi,0)*2; % in degrees
if edeflection > 20
  edeflection = 20;
end
if edeflection < -20
  edeflection = -20;
end
rdeflection = roundn(rdeflection/2  * 180/pi,0)*2; % in degrees
if rdeflection > 20
  rdeflection = 20;
end
if rdeflection < -20
  rdeflection = -20;
end
%}

for i=1:size(adb.data,1)
  if (roundn(adb.data(i,1),1) == beta && roundn(adb.data(i,2),1) == alpha)
    for j = 1:size(list,2)
      if isequal(output_selection{1},list{j})
        datalistA = adb.data(i,j+2);
        namelistA = list{j};
      end
    end
  end
end

for i=1:size(l_elevon.data,1)
  if (roundn(l_elevon.data(i,1),1) == min(20, max(-20, roundn(adeflection/2,0)*2 + edeflection)))
    for j = 1:size(list,2)
      if isequal(output_selection{1},list{j})
        datalistB = l_elevon.data(i,j+1);
        %namelistB = list{j};
      end
    end
  end
end

for i=1:size(r_elevon.data,1)
  if (roundn(r_elevon.data(i,1),1) == min(20, max(-20, -roundn(adeflection/2,0)*2 + edeflection)))
    for j = 1:size(list,2)
      if isequal(output_selection{1},list{j})
        datalistC = r_elevon.data(i,j+1);
        %namelistC = list{j};
      end
    end
  end
end

for i=1:size(rudder.data,1)
  if (roundn(rudder.data(i,1),1) == rdeflection)
    for j = 1:size(list,2)
      if isequal(output_selection{1},list{j})
        datalistD = rudder.data(i,j+1);
        %namelistD = list{j};
      end
    end
  end
end

%%

# plot(adb.data(514:532,2),adb.data(514:532,3))

outputA = [datalistA;datalistB;datalistC;datalistD]
outputB = [namelistA;namelistB;namelistC;namelistD]
