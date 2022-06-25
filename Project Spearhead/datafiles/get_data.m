clear;
clc;
pkg load mapping;

adb = importdata('mga_v2_adb.dat');
list = {'CFX','CFY','CFZ','CMX','CMY','CMZ',...
    'CL','CD','CS','CR','CM','CN'};

%% VARIABLES = "Beta [°]" "Alpha [°]" CFX CFY CFZ CMX CMY CMZ CL CD CS CR CM CN
%  ZONE T=MGI_woweapons_M3_MID, I=19, J=14, K=1
% As output, datalist gives the list of coefficients

output_type = 2; % 1 for all, 2 for specific force/moment (select from the list)
output_selection = {'CFZ'}; % if type is 2

beta = 0; % in degrees
alpha = 0; % in degrees

%%

for i=1:size(adb.data,1)
    if (roundn(adb.data(i,1),1) == beta && roundn(adb.data(i,2),1) == alpha)
        if (output_type == 1)
            datalist = transpose(adb.data(i,3:14));
            namelist = list;
        elseif (output_type == 2)
            for j = 1:size(list,2)
                if isequal(output_selection{1},list{j})
                    datalist = adb.data(i,j+2);
                    namelist = list{j};
                end
            end
        end
    end
end

%%
plot(adb.data(514:532,2),adb.data(514:532,3))
outputA = datalist
outputB = namelist
